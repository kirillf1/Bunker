namespace Bunker.Game.Domain.AggregateModels.Characters.Characteristics;

public class Trait : ValueObject
{
    public string Description { get; }

    public Trait(string description)
    {
        Description = description;
    }

    public override string ToString()
    {
        return "Черта характера: " + Description;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Description;
    }
}
