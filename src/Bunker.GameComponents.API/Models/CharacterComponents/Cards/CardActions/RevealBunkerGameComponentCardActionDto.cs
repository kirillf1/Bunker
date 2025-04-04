using Bunker.Domain.Shared.GameComponents;

namespace Bunker.GameComponents.API.Models.CharacterComponents.Cards.CardActions;

public class RevealBunkerGameComponentCardActionDto : CardActionDto
{
    public BunkerObjectType BunkerObjectType { get; set; }
}
