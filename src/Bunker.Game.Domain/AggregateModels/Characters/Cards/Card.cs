using Bunker.Game.Domain.AggregateModels.Characters.Cards.CardActions;

namespace Bunker.Game.Domain.AggregateModels.Characters.Cards;

public class Card : ValueObject
{
    public bool IsActivated { get; set; }

    public string Description { get; set; }

    public CardAction CardAction { get; set; }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        throw new NotImplementedException();
    }
}
