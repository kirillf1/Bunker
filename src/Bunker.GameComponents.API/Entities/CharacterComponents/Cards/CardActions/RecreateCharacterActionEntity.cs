namespace Bunker.GameComponents.API.Entities.CharacterComponents.Cards.CardActions
{
    public class RecreateCharacterActionEntity : CardActionEntity
    {
        public int TargetCharactersCount { get; set; }

        public RecreateCharacterActionEntity() { }

        public RecreateCharacterActionEntity(int targetCharactersCount)
            : base()
        {
            TargetCharactersCount = targetCharactersCount;
        }
    }
}
