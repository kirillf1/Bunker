using Bunker.Domain.Shared.GameComponents;
using Bunker.Game.Domain.AggregateModels.Characters.Cards.CardActions;

namespace Bunker.Domain.Shared.CardActionCommands;

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
