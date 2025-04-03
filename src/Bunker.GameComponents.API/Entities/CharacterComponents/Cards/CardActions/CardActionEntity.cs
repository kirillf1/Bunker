namespace Bunker.GameComponents.API.Entities.CharacterComponents.Cards.CardActions;

public abstract class CardActionEntity
{
    public Guid Id { get; set; }

    public CardEntity CardEntity { get; set; } = default!;

    protected CardActionEntity()
    {
        Id = Guid.CreateVersion7();
    }
}
