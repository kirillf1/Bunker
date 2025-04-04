using Microsoft.EntityFrameworkCore;

namespace Bunker.GameComponents.API.Infrastructure.Database;

public class GameComponentsDatabaseInitializer
{
    private readonly ILogger<GameComponentsDatabaseInitializer> _logger;
    private readonly GameComponentsContext _context;

    public GameComponentsDatabaseInitializer(
        ILogger<GameComponentsDatabaseInitializer> logger,
        GameComponentsContext context
    )
    {
        _logger = logger;
        _context = context;
    }

    public async Task InitializeAsync(CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Applying database migrations...");

        await _context.Database.MigrateAsync(cancellationToken);

        _logger.LogInformation("Migrations applied successfully.");
    }
}
