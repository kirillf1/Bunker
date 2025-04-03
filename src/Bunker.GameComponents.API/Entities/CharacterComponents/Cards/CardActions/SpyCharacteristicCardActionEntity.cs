using Bunker.Domain.Shared.GameComponents;

namespace Bunker.GameComponents.API.Entities.CharacterComponents.Cards.CardActions;

public class SpyCharacteristicCardActionEntity : CardActionEntity
{
    public CharacteristicType CharacteristicType { get; set; }
    public int TargetCharactersCount { get; set; }

    public SpyCharacteristicCardActionEntity() { }

    public SpyCharacteristicCardActionEntity(CharacteristicType characteristicType, int targetCharactersCount)
        : base()
    {
        CharacteristicType = characteristicType;
        TargetCharactersCount = targetCharactersCount;
    }
}
