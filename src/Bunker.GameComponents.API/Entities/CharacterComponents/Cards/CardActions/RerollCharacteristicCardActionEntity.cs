using Bunker.Domain.Shared.GameComponents;

namespace Bunker.GameComponents.API.Entities.CharacterComponents.Cards.CardActions
{
    public class RerollCharacteristicCardActionEntity : CardActionEntity
    {
        public CharacteristicType CharacteristicType { get; set; }
        public bool IsSelfTarget { get; set; }
        public Guid? CharacteristicId { get; set; }
        public int TargetCharactersCount { get; set; }

        public RerollCharacteristicCardActionEntity() { }

        public RerollCharacteristicCardActionEntity(
            CharacteristicType characteristicType,
            bool isSelfTarget,
            Guid? characteristicId,
            int targetCharactersCount
        )
            : base()
        {
            CharacteristicType = characteristicType;
            IsSelfTarget = isSelfTarget;
            CharacteristicId = characteristicId;
            TargetCharactersCount = targetCharactersCount;
        }
    }
}
