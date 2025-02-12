namespace Bunker.Game.Domain.AggregateModels.Bunkers;

public class Environment : ValueObject, IBunkerComponent<Environment>
{
    public string Description { get; }

    public bool IsHidden { get; }

    public Environment(string description)
    {
        Description = description;
        IsHidden = true;
    }

    public Environment(string description, bool isHidden)
    {
        Description = description;
        IsHidden = isHidden;
    }

    public Environment Reveal()
    {
        return new Environment(Description, false);
    }

    public string GetDescription()
    {
        return Description;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Description;
    }

#pragma warning disable CS8618
#pragma warning disable T0008
    protected Environment()
#pragma warning restore T0008
#pragma warning restore CS8618
    { }
}
