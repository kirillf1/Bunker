using Bunker.Domain.Shared.GameComponents;

namespace Bunker.GameComponents.API.Models.CharacterComponents.Cards.CardActions;

public class AddCharacteristicDto : CardActionDto
{
    public CharacteristicType CharacteristicType { get; set; }
    public Guid? CharacteristicId { get; set; }
    public int TargetCharactersCount { get; set; }
}
