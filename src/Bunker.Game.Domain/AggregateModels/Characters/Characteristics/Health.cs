namespace Bunker.Game.Domain.AggregateModels.Characters.Characteristics;

public class Health : ValueObject, ICharacteristic
{
    public string Description { get; }

    public Health(string description)
    {
        Description = description;
    }

#pragma warning disable CS8618
    private Health() { }
#pragma warning restore CS8618

    public string GetDescription()
    {
        return Description;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Description;
    }
}
