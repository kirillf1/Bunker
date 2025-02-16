using Bunker.Domain.Shared.GameComponents;
using Bunker.Game.Domain.AggregateModels.Characters.Cards.CardActions;

namespace Bunker.Domain.Shared.CardActionCommands;

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
