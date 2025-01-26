namespace Bunker.Game.Domain.AggregateModels.Characters.Characteristics;

public class Phobia : ValueObject
{
    public string Description { get; }

    public Phobia(string description)
    {
        Description = description;
    }

    private Phobia() { }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Description;
    }
}
