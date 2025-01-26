namespace Bunker.Game.Domain.AggregateModels.Characters.Characteristics;

public class Profession : ValueObject
{
    public string Description { get; }
    public byte Experience { get; }

    public Profession(string description, byte experience)
    {
        if (experience > 20)
            throw new ArgumentException("Experience must be less then 20");
        Description = description;
        Experience = experience;
    }

    private Profession() { }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Description;
        yield return Experience;
    }
}
