using System.Text.Json;
using System.Text.Json.Serialization;
using Bunker.GameComponents.API.Entities.BunkerComponents;
using Bunker.GameComponents.API.Entities.CatastropheComponents;
using Bunker.GameComponents.API.Entities.CharacterComponents;
using Bunker.GameComponents.API.Entities.CharacterComponents.Cards;
using Bunker.GameComponents.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Bunker.GameComponents.API.Infrastructure.Database;

public class GameComponentsDatabaseInitializer
{
    private const string DefaultGameComponentsFileName = "default_game_components.json";
    private const string InitializationLockKey = "GAME_COMPONENTS_INITIALIZATION";
    private const int LockTimeoutSeconds = 300; // 5 minutes timeout

    private readonly ILogger<GameComponentsDatabaseInitializer> _logger;
    private readonly GameComponentsContext _context;
    private readonly IWebHostEnvironment _environment;

    public GameComponentsDatabaseInitializer(
        ILogger<GameComponentsDatabaseInitializer> logger,
        GameComponentsContext context,
        IWebHostEnvironment environment
    )
    {
        _logger = logger;
        _context = context;
        _environment = environment;
    }

    public async Task InitializeAsync(CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Applying database migrations...");

        await _context.Database.MigrateAsync(cancellationToken);

        _logger.LogInformation("Migrations applied successfully.");

        _logger.LogInformation("Attempting to acquire initialization lock...");

        var lockAcquired = await TryAcquireInitializationLock(cancellationToken);

        if (!lockAcquired)
        {
            _logger.LogInformation("Another instance is initializing the database. Skipping initialization.");
            return;
        }

        try
        {
            _logger.LogInformation("Initialization lock acquired. Checking if data initialization is needed...");

            var hasData =
                await _context.Catastrophes.AnyAsync(cancellationToken)
                || await _context.BunkerDescriptions.AnyAsync(cancellationToken)
                || await _context.Professions.AnyAsync(cancellationToken);

            if (!hasData)
            {
                _logger.LogInformation("Database is empty. Loading data from JSON file...");
                await LoadDefaultDataAsync(cancellationToken);
                _logger.LogInformation("Data from JSON file loaded successfully.");
            }
            else
            {
                _logger.LogInformation("Database already contains data. Skipping initialization.");
            }
        }
        finally
        {
            await ReleaseInitializationLock(cancellationToken);
            _logger.LogInformation("Initialization lock released.");
        }
    }

    private async Task<bool> TryAcquireInitializationLock(CancellationToken cancellationToken)
    {
        try
        {
            await EnsureInitializationLockTableExists(cancellationToken);

            var lockExpiry = DateTime.UtcNow.AddSeconds(LockTimeoutSeconds);
            var instanceId = Environment.MachineName + "-" + Environment.ProcessId;

            var rowsAffected = await _context.Database.ExecuteSqlRawAsync(
                @"
                INSERT INTO initialization_locks (lock_key, locked_at, expires_at, instance_id) 
                VALUES ({0}, {1}, {2}, {3})
                ON CONFLICT (lock_key) DO UPDATE SET
                    locked_at = EXCLUDED.locked_at,
                    expires_at = EXCLUDED.expires_at,
                    instance_id = EXCLUDED.instance_id
                WHERE initialization_locks.expires_at < {1}",
                InitializationLockKey,
                DateTime.UtcNow,
                lockExpiry,
                instanceId
            );

            return rowsAffected > 0;
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to acquire initialization lock. Proceeding without lock.");
            return true;
        }
    }

    private async Task ReleaseInitializationLock(CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        try
        {
            var instanceId = Environment.MachineName + "-" + Environment.ProcessId;

            await _context.Database.ExecuteSqlRawAsync(
                @"
                DELETE FROM initialization_locks 
                WHERE lock_key = {0} AND instance_id = {1}",
                InitializationLockKey,
                instanceId
            );
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to release initialization lock.");
        }
    }

    private async Task EnsureInitializationLockTableExists(CancellationToken cancellationToken)
    {
        try
        {
            await _context.Database.ExecuteSqlRawAsync(
                @"
                CREATE TABLE IF NOT EXISTS initialization_locks (
                    lock_key VARCHAR(100) PRIMARY KEY,
                    locked_at TIMESTAMP WITH TIME ZONE NOT NULL,
                    expires_at TIMESTAMP WITH TIME ZONE NOT NULL,
                    instance_id VARCHAR(200) NOT NULL
                )",
                cancellationToken
            );
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to create initialization_locks table.");
        }
    }

    private async Task LoadDefaultDataAsync(CancellationToken cancellationToken)
    {
        var jsonFilePath = Path.Combine(_environment.ContentRootPath, DefaultGameComponentsFileName);

        if (!File.Exists(jsonFilePath))
        {
            _logger.LogWarning(
                "File {DefaultFileName} not found at path: {FilePath}",
                DefaultGameComponentsFileName,
                jsonFilePath
            );
            return;
        }

        var jsonContent = await File.ReadAllTextAsync(jsonFilePath, cancellationToken);

        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            Converters = { new JsonStringEnumConverter() },
        };

        var gameComponents = JsonSerializer.Deserialize<DefaultGameComponentsModel>(jsonContent, options);

        if (gameComponents == null)
        {
            _logger.LogError("Failed to deserialize JSON file");
            return;
        }

        foreach (var catastrophe in gameComponents.Catastrophes)
        {
            _context.Catastrophes.Add(new CatastropheEntity(catastrophe));
        }

        foreach (var description in gameComponents.BunkerDescriptions)
        {
            _context.BunkerDescriptions.Add(new BunkerDescriptionEntity(description));
        }

        foreach (var room in gameComponents.BunkerRooms)
        {
            _context.BunkerRooms.Add(new RoomEntity(room));
        }

        foreach (var environment in gameComponents.BunkerEnvironments)
        {
            _context.BunkerEnvironments.Add(new EnvironmentEntity(environment));
        }

        foreach (var item in gameComponents.BunkerItems)
        {
            _context.BunkerItems.Add(new BunkerItemEntity(item));
        }

        foreach (var profession in gameComponents.Professions)
        {
            _context.Professions.Add(new ProfessionEntity(profession));
        }

        foreach (var phobia in gameComponents.Phobias)
        {
            _context.Phobias.Add(new PhobiaEntity(phobia));
        }

        foreach (var additionalInfo in gameComponents.AdditionalInformations)
        {
            _context.AdditionalInformationEntitles.Add(new AdditionalInformationEntity(additionalInfo));
        }

        foreach (var health in gameComponents.Health)
        {
            _context.HealthEntitles.Add(new HealthEntity(health));
        }

        foreach (var trait in gameComponents.Traits)
        {
            _context.Traits.Add(new TraitEntity(trait));
        }

        foreach (var hobby in gameComponents.Hobbies)
        {
            _context.Hobbies.Add(new HobbyEntity(hobby));
        }

        foreach (var item in gameComponents.Items)
        {
            _context.Items.Add(new ItemEntity(item));
        }

        foreach (var card in gameComponents.Cards)
        {
            _context.Cards.Add(new CardEntity(card.Description, card.CardAction));
        }

        await _context.SaveChangesAsync(cancellationToken);

        _logger.LogInformation(
            "Loaded {CatastropheCount} catastrophes, {BunkerDescriptionCount} bunker descriptions, "
                + "{ProfessionCount} professions, {CardCount} cards and other components",
            gameComponents.Catastrophes.Count,
            gameComponents.BunkerDescriptions.Count,
            gameComponents.Professions.Count,
            gameComponents.Cards.Count
        );
    }
}
