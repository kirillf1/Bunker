using Bunker.Domain.Shared.GameComponents;

namespace Bunker.Domain.Shared.Cards.CardActionCommands;

public class RerollCharacteristicActionCommand : CardActionCommand
{
    public CharacteristicType CharacteristicType { get; }
    public IEnumerable<Guid> TargetCharactersIds { get; }
    public Guid? CharacteristicId { get; }
    public bool IsSelfTarget { get; }

    public RerollCharacteristicActionCommand(
        CharacteristicType characteristicType,
        IEnumerable<Guid> targetCharactersIds,
        Guid? characteristicId,
        bool isSelfTarget
    )
    {
        CharacteristicType = characteristicType;
        TargetCharactersIds = targetCharactersIds;
        CharacteristicId = characteristicId;
        IsSelfTarget = isSelfTarget;
    }
}
