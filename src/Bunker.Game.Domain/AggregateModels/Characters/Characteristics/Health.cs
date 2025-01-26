namespace Bunker.Game.Domain.AggregateModels.Characters.Characteristics;

public class Health : ValueObject
{
    public string Description { get; }

    public Health(string description)
    {
        Description = description;
    }

#pragma warning disable CS8618
    private Health() { }
#pragma warning restore CS8618

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Description;
    }
}
