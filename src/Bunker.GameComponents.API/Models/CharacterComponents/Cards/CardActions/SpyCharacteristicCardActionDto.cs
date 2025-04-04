using Bunker.Domain.Shared.GameComponents;

namespace Bunker.GameComponents.API.Models.CharacterComponents.Cards.CardActions;

public class SpyCharacteristicCardActionDto : CardActionDto
{
    public CharacteristicType CharacteristicType { get; set; }
    public int TargetCharactersCount { get; set; }
}
