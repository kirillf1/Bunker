using Bunker.Domain.Shared.GameComponents;

namespace Bunker.GameComponents.API.Models.CharacterComponents.Cards.CardActions;

public class RerollCharacteristicCardActionDto : CardActionDto
{
    public CharacteristicType CharacteristicType { get; set; }
    public bool IsSelfTarget { get; set; }
    public Guid? CharacteristicId { get; set; }
    public int TargetCharactersCount { get; set; }
}
