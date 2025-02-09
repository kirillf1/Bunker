namespace Bunker.Game.Domain.AggregateModels.Characters.Cards;

public class ActivateCardParams : ValueObject
{
    public IEnumerable<Guid> TargetCharacterIds { get; private set; }

    public ActivateCardParams()
    {
        TargetCharacterIds = [];
    }

    public ActivateCardParams(IEnumerable<Guid> targetCharacterIds)
    {
        TargetCharacterIds = new HashSet<Guid>(targetCharacterIds);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return TargetCharacterIds;
    }
}
