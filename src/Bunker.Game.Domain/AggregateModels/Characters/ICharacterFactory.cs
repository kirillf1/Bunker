namespace Bunker.Game.Domain.AggregateModels.Characters;

public interface ICharacterFactory
{
    Task<Character> CreateCharacter(Guid gameSessionId);

    Task<IEnumerable<Character>> CreateCharacters(Guid gameSessionId, int count);
}
