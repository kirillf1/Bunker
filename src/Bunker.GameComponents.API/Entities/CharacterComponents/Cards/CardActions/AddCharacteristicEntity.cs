using Bunker.Domain.Shared.GameComponents;

namespace Bunker.GameComponents.API.Entities.CharacterComponents.Cards.CardActions
{
    public class AddCharacteristicEntity : CardActionEntity
    {
        public CharacteristicType CharacteristicType { get; set; }
        public Guid? CharacteristicId { get; set; }
        public int TargetCharactersCount { get; set; }

        public AddCharacteristicEntity() { }

        public AddCharacteristicEntity(
            CharacteristicType characteristicType,
            Guid? characteristicId,
            int targetCharactersCount
        )
            : base()
        {
            CharacteristicType = characteristicType;
            CharacteristicId = characteristicId;
            TargetCharactersCount = targetCharactersCount;
        }
    }
}
