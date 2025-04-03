using Bunker.Domain.Shared.GameComponents;

namespace Bunker.GameComponents.API.Entities.CharacterComponents.Cards.CardActions;

public class RemoveCharacteristicCardActionEntity : CardActionEntity
{
    public CharacteristicType CharacteristicType { get; set; }
    public int TargetCharactersCount { get; set; }

    public RemoveCharacteristicCardActionEntity() { }

    public RemoveCharacteristicCardActionEntity(CharacteristicType characteristicType, int targetCharactersCount)
        : base()
    {
        CharacteristicType = characteristicType;
        TargetCharactersCount = targetCharactersCount;
    }
}
