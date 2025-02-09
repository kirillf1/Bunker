using Bunker.Game.Domain.AggregateModels.Characters.Characteristics;

namespace Bunker.Game.Domain.AggregateModels.Characters;

public class Character : Entity<Guid>, IAggregateRoot
{
    private readonly List<Item> _items;

    private readonly List<Trait> _traits;

    public Guid GameSessionId { get; private set; }
    public AdditionalInformation AdditionalInformation { get; private set; }

    public Age Age { get; private set; }

    public Childbearing Childbearing { get; private set; }

    public Health Health { get; private set; }

    public Hobby Hobby { get; private set; }

    public Phobia Phobia { get; private set; }

    public Profession Profession { get; private set; }

    public Sex Sex { get; private set; }

    public bool IsKicked { get; private set; }

    public IReadOnlyCollection<Trait> Traits => _traits;

    public IReadOnlyCollection<Item> Items => _items;

    public Character(
        Guid id,
        Guid gameSessionId,
        AdditionalInformation additionalInformation,
        Age age,
        Childbearing childbearing,
        Health health,
        Hobby hobby,
        Phobia phobia,
        Profession profession,
        Sex sex,
        IEnumerable<Item> items,
        IEnumerable<Trait> traits
    )
        : base(id)
    {
        GameSessionId = gameSessionId;
        AdditionalInformation = additionalInformation;
        Age = age;
        Childbearing = childbearing;
        Health = health;
        Hobby = hobby;
        Phobia = phobia;
        Profession = profession;
        Sex = sex;
        IsKicked = false;

        if (!items.Any())
            throw new ArgumentException("Character must have one or more items");

        _items = new List<Item>(items);

        if (!traits.Any())
            throw new ArgumentException("Character must have one or more traits");

        _traits = new List<Trait>(traits);
    }

    public void MarkKicked()
    {
        IsKicked = true;
    }

    public void UpdateAdditionalInformation(AdditionalInformation additionalInformation)
    {
        AdditionalInformation = additionalInformation ?? throw new ArgumentNullException(nameof(additionalInformation));
    }

    public void UpdateAge(Age age)
    {
        Age = age ?? throw new ArgumentNullException(nameof(age));
    }

    public void UpdateChildbearing(Childbearing childbearing)
    {
        Childbearing = childbearing ?? throw new ArgumentNullException(nameof(childbearing));
    }

    public void UpdateHealth(Health health)
    {
        Health = health ?? throw new ArgumentNullException(nameof(health));
    }

    public void UpdateHobby(Hobby hobby)
    {
        Hobby = hobby ?? throw new ArgumentNullException(nameof(hobby));
    }

    public void UpdatePhobia(Phobia phobia)
    {
        Phobia = phobia ?? throw new ArgumentNullException(nameof(phobia));
    }

    public void UpdateProfession(Profession profession)
    {
        Profession = profession ?? throw new ArgumentNullException(nameof(profession));
    }

    public void UpdateSex(Sex sex)
    {
        Sex = sex ?? throw new ArgumentNullException(nameof(sex));
    }

    public void AddItem(Item item)
    {
        ArgumentNullException.ThrowIfNull(item);

        _items.Add(item);
    }

    public void RemoveItem(Item item)
    {
        ArgumentNullException.ThrowIfNull(item);

        _items.Remove(item);
    }

    public void ReplaceItem(Item oldItem, Item newItem)
    {
        ArgumentNullException.ThrowIfNull(oldItem);
        ArgumentNullException.ThrowIfNull(newItem);

        var index = _items.IndexOf(oldItem);
        if (index == -1)
            throw new InvalidGameOperationException("Item to replace not found");

        _items[index] = newItem;
    }

    public void AddTrait(Trait trait)
    {
        ArgumentNullException.ThrowIfNull(trait);

        _traits.Add(trait);
    }

    public void RemoveTrait(Trait trait)
    {
        ArgumentNullException.ThrowIfNull(trait);

        _traits.Remove(trait);
    }

    public void ReplaceTrait(Trait oldTrait, Trait newTrait)
    {
        ArgumentNullException.ThrowIfNull(oldTrait);
        ArgumentNullException.ThrowIfNull(newTrait);

        var index = _traits.IndexOf(oldTrait);
        if (index == -1)
            throw new InvalidGameOperationException("Trait to replace not found");

        _traits[index] = newTrait;
    }

#pragma warning disable CS8618
    private Character(Guid id)
        : base(id) { }
#pragma warning disable CS8618
}
