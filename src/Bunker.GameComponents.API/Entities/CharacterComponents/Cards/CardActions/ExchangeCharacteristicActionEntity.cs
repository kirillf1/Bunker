using Bunker.Domain.Shared.GameComponents;

namespace Bunker.GameComponents.API.Entities.CharacterComponents.Cards.CardActions;

public class ExchangeCharacteristicActionEntity : CardActionEntity
{
    public CharacteristicType CharacteristicType { get; set; }

    public ExchangeCharacteristicActionEntity() { }

    public ExchangeCharacteristicActionEntity(CharacteristicType characteristicType)
        : base()
    {
        CharacteristicType = characteristicType;
    }
}
