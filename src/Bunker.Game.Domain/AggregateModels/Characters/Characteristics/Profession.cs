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

#pragma warning disable CS8618 // Поле, не допускающее значения NULL, должно содержать значение, отличное от NULL, при выходе из конструктора. Рассмотрите возможность добавления модификатора "required" или объявления значения, допускающего значение NULL.
    private Profession() { }
#pragma warning restore CS8618 // Поле, не допускающее значения NULL, должно содержать значение, отличное от NULL, при выходе из конструктора. Рассмотрите возможность добавления модификатора "required" или объявления значения, допускающего значение NULL.

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Description;
        yield return Experience;
    }
}
