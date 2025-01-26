namespace Bunker.Game.Domain.AggregateModels.Characters.Characteristics;

public class Childbearing : ValueObject
{
    public bool CanGiveBirth { get; }

    public Childbearing(bool canGiveBirth)
    {
        CanGiveBirth = canGiveBirth;
    }

    private Childbearing() { }

    public override string ToString()
    {
        return "Деторождение: " + (CanGiveBirth ? "не childfree" : "childfree");
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return CanGiveBirth;
    }
}
