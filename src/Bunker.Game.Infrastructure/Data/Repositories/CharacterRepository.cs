using Bunker.Game.Domain.AggregateModels.Characters;
using Microsoft.EntityFrameworkCore;

namespace Bunker.Game.Infrastructure.Data.Repositories;

public class CharacterRepository : RepositoryBase<Character>, ICharacterRepository
{
    public CharacterRepository(BunkerGameDbContext context)
        : base(context) { }

    public async Task<Character?> GetCharacter(Guid id)
    {
        return await _context.Set<Character>().FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<IEnumerable<Character>> GetCharactersByGameSession(Guid gameSessionId)
    {
        return await _context.Set<Character>().Where(c => c.GameSessionId == gameSessionId).ToListAsync();
    }
}
