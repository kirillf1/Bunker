namespace Bunker.Game.Domain.AggregateModels.GameSessions;

public class Catastrophe : ValueObject
{
    public string Description { get; }

    public Catastrophe(string description)
    {
        Description = description;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Description;
    }
}
