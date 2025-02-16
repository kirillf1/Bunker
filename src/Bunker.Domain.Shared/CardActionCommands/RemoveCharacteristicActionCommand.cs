using Bunker.Domain.Shared.GameComponents;
using Bunker.Game.Domain.AggregateModels.Characters.Cards.CardActions;

namespace Bunker.Domain.Shared.CardActionCommands;

public class RemoveCharacteristicActionCommand : CardActionCommand
{
    public CharacteristicType CharacteristicType { get; }
    public IEnumerable<Guid> TargetCharactersIds { get; }

    public RemoveCharacteristicActionCommand(CharacteristicType characteristicType, IEnumerable<Guid> targetCharactersIds)
    {
        CharacteristicType = characteristicType;
        TargetCharactersIds = targetCharactersIds;
    }
}
