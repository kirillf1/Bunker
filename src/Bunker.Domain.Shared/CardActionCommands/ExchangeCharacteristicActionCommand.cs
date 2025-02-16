using Bunker.Domain.Shared.GameComponents;
using Bunker.Game.Domain.AggregateModels.Characters.Cards.CardActions;

namespace Bunker.Domain.Shared.CardActionCommands;

public class ExchangeCharacteristicActionCommand : CardActionCommand
{
    public CharacteristicType CharacteristicType { get; }

    public ExchangeCharacteristicActionCommand(CharacteristicType characteristicType)
    {
        CharacteristicType = characteristicType;
    }
}
