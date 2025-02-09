namespace Bunker.Game.Domain.AggregateModels.Characters.Cards;

public class CardActionRequirements : ValueObject
{
    public ActivateCardTargetType ActivateCardTargetType { get; init; }
    public int Count { get; init; }

    public CardActionRequirements(ActivateCardTargetType activateCardTargetType, int count)
    {
        ActivateCardTargetType = activateCardTargetType;
        Count = count;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Count;
        yield return ActivateCardTargetType;
    }
}

public enum ActivateCardTargetType
{
    None = 0,
    Character = 1,
}
