namespace Bunker.Game.Domain.AggregateModels.Characters.Characteristics;

public class Sex : ValueObject, ICharacteristic
{
    public string Description { get; }

    public Sex(string description)
    {
        Description = description;
    }

    public override string ToString()
    {
        return $"Пол: {Description}";
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
