using Bunker.Game.Domain.AggregateModels.Catastrophes;
using Microsoft.EntityFrameworkCore;

namespace Bunker.Game.Infrastructure.Data.Repositories;

public class CatastropheRepository : RepositoryBase<Catastrophe>, ICatastropheRepository
{
    public CatastropheRepository(BunkerGameDbContext context)
        : base(context) { }

    public async Task<Catastrophe?> GetCatastrophe(Guid id)
    {
        return await _context.Catastrophes.FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<Catastrophe?> GetCatastropheByGameSession(Guid gameSessionId)
    {
        return await _context.Catastrophes.FirstOrDefaultAsync(c => c.GameSessionId == gameSessionId);
    }
}
