using Bunker.Game.Domain.AggregateModels.GameSessions;
using Microsoft.EntityFrameworkCore;

namespace Bunker.Game.Infrastructure.Data.Repositories;

public class GameSessionRepository : RepositoryBase<GameSession>, IGameSessionRepository
{
    public GameSessionRepository(BunkerGameDbContext context)
        : base(context) { }

    public async Task<GameSession?> GetGameSession(Guid id)
    {
        return await _context.GameSessions.FirstOrDefaultAsync(gs => gs.Id == id);
    }
}
