namespace Bunker.Game.Domain.AggregateModels.Characters;

public interface ICharacterRepository : IRepository<Character>
{
    Task<Character?> GetCharacter(Guid id);

    Task<IEnumerable<Character>> GetCharactersByGameSession(Guid gameSessionId);
}
