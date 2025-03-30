using Bunker.Domain.Shared.GameComponents;

namespace Bunker.Domain.Shared.Cards.CardActionCommands;

public class SpyCharacteristicActionCommand : CardActionCommand
{
    public CharacteristicType CharacteristicType { get; }
    public IEnumerable<Guid> TargetCharactersIds { get; }

    public SpyCharacteristicActionCommand(CharacteristicType characteristicType, IEnumerable<Guid> targetCharactersIds)
    {
        CharacteristicType = characteristicType;
        TargetCharactersIds = targetCharactersIds;
    }
}
