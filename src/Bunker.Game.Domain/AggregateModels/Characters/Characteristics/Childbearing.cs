namespace Bunker.Game.Domain.AggregateModels.Characters.Characteristics;

public class Childbearing : ValueObject, ICharacteristic
{
    public bool CanGiveBirth { get; }

    public Childbearing(bool canGiveBirth)
    {
        CanGiveBirth = canGiveBirth;
    }

    private Childbearing() { }

    public string GetDescription()
    {
        return CanGiveBirth ? "no childfree" : "childfree";
    }

    public override string ToString()
    {
        return "Деторождение: " + (CanGiveBirth ? "не childfree" : "childfree");
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return CanGiveBirth;
    }
}
