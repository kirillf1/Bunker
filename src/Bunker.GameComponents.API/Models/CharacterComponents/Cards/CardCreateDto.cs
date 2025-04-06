using Bunker.GameComponents.API.Entities.CharacterComponents.Cards.CardActions;

namespace Bunker.GameComponents.API.Models.CharacterComponents.Cards;

public class CardCreateDto
{
    public required string Description { get; set; }
    public required CardActionEntity CardAction { get; set; }
}
