namespace Bunker.Game.Domain.AggregateModels.Characters.Characteristics;

public class Item : ValueObject, ICharacteristic
{
    public string Description { get; }

    public Item(string description)
    {
        Description = description;
    }

#pragma warning disable CS8618
    private Item() { }
#pragma warning restore CS8618

    public override string ToString()
    {
        return Description;
    }

    public string GetDescription()
    {
        return Description;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Description;
    }
}
