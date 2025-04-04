using Bunker.Domain.Shared.GameComponents;

namespace Bunker.GameComponents.API.Models.CharacterComponents.Cards.CardActions;

public class ExchangeCharacteristicActionDto : CardActionDto
{
    public CharacteristicType CharacteristicType { get; set; }
}
