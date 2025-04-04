using Bunker.GameComponents.API.Models.CharacterComponents.Cards.CardActions;

namespace Bunker.GameComponents.API.Models.CharacterComponents.Cards;

public class CardDto
{
    public Guid Id { get; set; }
    public required string Description { get; set; }
    public required CardActionDto CardAction { get; set; }
}
