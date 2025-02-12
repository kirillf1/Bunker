namespace Bunker.Game.Domain.AggregateModels.Characters;

public interface ICharacterGenerator
{
    Task<Character> GenerateCharacter(Guid gameSessionId);

    Task<IEnumerable<Character>> GenerateCharacters(Guid gameSessionId, int count);
}
