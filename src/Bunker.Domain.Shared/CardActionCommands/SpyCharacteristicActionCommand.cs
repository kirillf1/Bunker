using Bunker.Domain.Shared.GameComponents;
using Bunker.Game.Domain.AggregateModels.Characters.Cards.CardActions;

namespace Bunker.Domain.Shared.CardActionCommands;

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
