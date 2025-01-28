namespace Bunker.Game.Domain.AggregateModels.Characters.Characteristics;

public class Phobia : ValueObject
{
    public string Description { get; }

    public Phobia(string description)
    {
        Description = description;
    }

#pragma warning disable CS8618 // Поле, не допускающее значения NULL, должно содержать значение, отличное от NULL, при выходе из конструктора. Рассмотрите возможность добавления модификатора "required" или объявления значения, допускающего значение NULL.
    private Phobia() { }
#pragma warning restore CS8618 // Поле, не допускающее значения NULL, должно содержать значение, отличное от NULL, при выходе из конструктора. Рассмотрите возможность добавления модификатора "required" или объявления значения, допускающего значение NULL.

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Description;
    }
}
