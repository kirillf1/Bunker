namespace Bunker.Game.Domain.AggregateModels.Characters.Characteristics;

public class Age : ValueObject
{
    public int Years { get; }

    public Age(int years)
    {
        if (years <= 16)
            throw new ArgumentException("Age must be more than 16");
        Years = years;
    }

    public Age()
    {
        Years = 16;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Years;
    }
}
