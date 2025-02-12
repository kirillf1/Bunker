namespace Bunker.Game.Domain.AggregateModels.Bunkers;

public class Item : ValueObject, IBunkerComponent<Item>
{
    public string Description { get; }

    public bool IsHidden { get; }

    public Item(string description)
    {
        Description = description;
        IsHidden = true;
    }

    public Item(string description, bool isHidden)
    {
        Description = description;
        IsHidden = isHidden;
    }

    public string GetDescription()
    {
        return Description;
    }

    public Item Reveal()
    {
        return new Item(Description, false);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Description;
    }

#pragma warning disable CS8618 // Поле, не допускающее значения NULL, должно содержать значение, отличное от NULL, при выходе из конструктора. Рассмотрите возможность добавления модификатора "required" или объявления значения, допускающего значение NULL.
#pragma warning disable T0008 // Internal Styling Rule T0008
    protected Item()
#pragma warning restore T0008 // Internal Styling Rule T0008
#pragma warning restore CS8618 // Поле, не допускающее значения NULL, должно содержать значение, отличное от NULL, при выходе из конструктора. Рассмотрите возможность добавления модификатора "required" или объявления значения, допускающего значение NULL.
    { }
}
