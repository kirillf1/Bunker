namespace Bunker.Game.Domain.AggregateModels.Characters.Characteristics;

public class Trait : ValueObject, ICharacteristic
{
    public string Description { get; }

    public Trait(string description)
    {
        Description = description;
    }

    public string GetDescription()
    {
        return Description;
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
