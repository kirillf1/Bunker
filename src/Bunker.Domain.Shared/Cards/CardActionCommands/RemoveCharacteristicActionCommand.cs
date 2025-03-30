using Bunker.Domain.Shared.GameComponents;

namespace Bunker.Domain.Shared.Cards.CardActionCommands;

public class RemoveCharacteristicActionCommand : CardActionCommand
{
    public CharacteristicType CharacteristicType { get; }
    public IEnumerable<Guid> TargetCharactersIds { get; }

    public RemoveCharacteristicActionCommand(
        CharacteristicType characteristicType,
        IEnumerable<Guid> targetCharactersIds
    )
    {
        CharacteristicType = characteristicType;
        TargetCharactersIds = targetCharactersIds;
    }
}
