using Bunker.Domain.Shared.GameComponents;

namespace Bunker.GameComponents.API.Entities.CharacterComponents.Cards.CardActions
{
    public class RevealBunkerGameComponentCardActionEntity : CardActionEntity
    {
        public BunkerObjectType BunkerObjectType { get; set; }

        public RevealBunkerGameComponentCardActionEntity() { }

        public RevealBunkerGameComponentCardActionEntity(BunkerObjectType bunkerObjectType)
            : base()
        {
            BunkerObjectType = bunkerObjectType;
        }
    }
}
