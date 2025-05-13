using Bunker.Game.Domain.AggregateModels.Bunkers;
using Microsoft.EntityFrameworkCore;

namespace Bunker.Game.Infrastructure.Data.Repositories;

public class BunkerRepository : RepositoryBase<BunkerAggregate>, IBunkerRepository
{
    public BunkerRepository(BunkerGameDbContext context)
        : base(context) { }

    public async Task<BunkerAggregate?> GetBunker(Guid id)
    {
        return await _context.Bunkers.FirstOrDefaultAsync(b => b.Id == id);
    }

    public async Task<BunkerAggregate?> GetBunkerByGameSessionId(Guid gameSessionId)
    {
        return await _context.Bunkers.FirstOrDefaultAsync(b => b.GameSessionId == gameSessionId);
    }
}
