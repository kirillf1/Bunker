using Bunker.GameComponents.API.Entities.CharacterComponents.Cards.CardActions;

namespace Bunker.GameComponents.API.Models.CharacterComponents.Cards;

public class CardDto
{
    public Guid Id { get; set; }
    public required string Description { get; set; }
    public required CardActionEntity CardAction { get; set; }
}
