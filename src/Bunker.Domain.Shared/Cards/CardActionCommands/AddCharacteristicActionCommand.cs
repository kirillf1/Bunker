using Bunker.Domain.Shared.GameComponents;

namespace Bunker.Domain.Shared.Cards.CardActionCommands;

public class AddCharacteristicActionCommand : CardActionCommand
{
    public CharacteristicType CharacteristicType { get; }
    public IEnumerable<Guid> TargetCharactersIds { get; }
    public Guid? CharacteristicId { get; }

    public AddCharacteristicActionCommand(
        CharacteristicType characteristicType,
        IEnumerable<Guid> targetCharactersIds,
        Guid? characteristicId
    )
    {
        CharacteristicType = characteristicType;
        TargetCharactersIds = targetCharactersIds;
        CharacteristicId = characteristicId;
    }
}
