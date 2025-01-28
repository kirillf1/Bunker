namespace Bunker.Game.Domain.AggregateModels.Characters.Cards;

public class Card : ValueObject
{
    protected override IEnumerable<object> GetEqualityComponents()
    {
        throw new NotImplementedException();
    }
}
