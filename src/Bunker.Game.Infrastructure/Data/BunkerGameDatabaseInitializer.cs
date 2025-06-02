using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Bunker.Game.Infrastructure.Data;

public class BunkerGameDatabaseInitializer
{
    private readonly ILogger<BunkerGameDatabaseInitializer> _logger;
    private readonly BunkerGameDbContext _context;

    public BunkerGameDatabaseInitializer(
        ILogger<BunkerGameDatabaseInitializer> logger,
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
