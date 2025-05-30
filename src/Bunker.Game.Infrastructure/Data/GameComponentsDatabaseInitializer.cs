using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Bunker.Game.Infrastructure.Data;

public class GameComponentsDatabaseInitializer
{
    private readonly ILogger<GameComponentsDatabaseInitializer> _logger;
    private readonly BunkerGameDbContext _context;

    public GameComponentsDatabaseInitializer(
        ILogger<GameComponentsDatabaseInitializer> logger,
        BunkerGameDbContext context
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
