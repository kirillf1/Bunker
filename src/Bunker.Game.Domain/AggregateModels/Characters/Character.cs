using Bunker.Domain.Shared.Cards.CardActionCommands;
using Bunker.Game.Domain.AggregateModels.Characters.Cards;
using Bunker.Game.Domain.AggregateModels.Characters.Characteristics;
using Bunker.Game.Domain.AggregateModels.Characters.Events;

namespace Bunker.Game.Domain.AggregateModels.Characters;

public class Character : Entity<Guid>, IAggregateRoot
{
    public const int MAX_TRAITS_IN_START_GAME = 2;
    public const int MIN_TRAITS_IN_START_GAME = 1;
    public const int MAX_CARDS_IN_START_GAME = 2;
    public const int MIN_CARDS_IN_START_GAME = 2;
    public const int MAX_ITEMS_IN_START_GAME = 2;
    public const int MIN_ITEMS_IN_START_GAME = 1;

    private List<Item> _items;
    private List<Trait> _traits;
    private List<Card> _cards;

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

    public IReadOnlyCollection<Card> Cards => _cards;

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
        IEnumerable<Trait> traits,
        IEnumerable<Card> cards
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

        var itemsCount = items.Count();
        if (itemsCount < MIN_ITEMS_IN_START_GAME || itemsCount > MAX_ITEMS_IN_START_GAME)
        {
            throw new ArgumentException(
                $"Invalid items count. Min items count {MIN_ITEMS_IN_START_GAME}, Max items count {MAX_ITEMS_IN_START_GAME}"
            );
        }
        _items = new List<Item>(items);

        var traitsCount = traits.Count();
        if (traitsCount < MIN_TRAITS_IN_START_GAME || traitsCount > MAX_TRAITS_IN_START_GAME)
        {
            throw new ArgumentException(
                $"Invalid traits count. Min traits count {MIN_TRAITS_IN_START_GAME}, Max traits count {MAX_TRAITS_IN_START_GAME}"
            );
        }
        _traits = new List<Trait>(traits);

        var cardsCount = cards.Count();
        if (cardsCount < MIN_CARDS_IN_START_GAME || cardsCount > MAX_CARDS_IN_START_GAME)
        {
            throw new ArgumentException(
                $"Invalid cards count. Min cards count {MIN_CARDS_IN_START_GAME}, Max cards count {MAX_CARDS_IN_START_GAME}"
            );
        }
        _cards = new List<Card>(cards);
    }

    public void RecreateCharacter(
        AdditionalInformation additionalInformation,
        Age age,
        Childbearing childbearing,
        Health health,
        Hobby hobby,
        Phobia phobia,
        Profession profession,
        Sex sex,
        IEnumerable<Item> items,
        IEnumerable<Trait> traits,
        IEnumerable<Card> cards
    )
    {
        AdditionalInformation = additionalInformation;
        Age = age;
        Childbearing = childbearing;
        Health = health;
        Hobby = hobby;
        Phobia = phobia;
        Profession = profession;
        Sex = sex;
        _items = new List<Item>(items);
        _cards = new List<Card>(cards);
        _traits = new List<Trait>(traits);

        AddDomainEvent(
            new CharacterRecreatedDomainEvent(
                Id,
                GameSessionId,
                additionalInformation,
                age,
                childbearing,
                health,
                hobby,
                phobia,
                profession,
                sex,
                items,
                traits,
                cards
            )
        );
    }

    public void MarkKicked()
    {
        IsKicked = true;
    }

    public void UpdateAdditionalInformation(AdditionalInformation additionalInformation)
    {
        AdditionalInformation = additionalInformation ?? throw new ArgumentNullException(nameof(additionalInformation));

        AddDomainEvent(new CharacteristicUpdatedDomainEvent(Id, AdditionalInformation));
    }

    public void UpdateAge(Age age)
    {
        Age = age ?? throw new ArgumentNullException(nameof(age));

        AddDomainEvent(new CharacteristicUpdatedDomainEvent(Id, Age));
    }

    public void UpdateChildbearing(Childbearing childbearing)
    {
        Childbearing = childbearing ?? throw new ArgumentNullException(nameof(childbearing));

        AddDomainEvent(new CharacteristicUpdatedDomainEvent(Id, Childbearing));
    }

    public void UpdateHealth(Health health)
    {
        Health = health ?? throw new ArgumentNullException(nameof(health));

        AddDomainEvent(new CharacteristicUpdatedDomainEvent(Id, Health));
    }

    public void UpdateHobby(Hobby hobby)
    {
        Hobby = hobby ?? throw new ArgumentNullException(nameof(hobby));

        AddDomainEvent(new CharacteristicUpdatedDomainEvent(Id, Hobby));
    }

    public void UpdatePhobia(Phobia phobia)
    {
        Phobia = phobia ?? throw new ArgumentNullException(nameof(phobia));

        AddDomainEvent(new CharacteristicUpdatedDomainEvent(Id, Phobia));
    }

    public void UpdateProfession(Profession profession)
    {
        Profession = profession ?? throw new ArgumentNullException(nameof(profession));

        AddDomainEvent(new CharacteristicUpdatedDomainEvent(Id, Profession));
    }

    public void UpdateSex(Sex sex)
    {
        Sex = sex ?? throw new ArgumentNullException(nameof(sex));

        AddDomainEvent(new CharacteristicUpdatedDomainEvent(Id, Sex));
    }

    public void AddItem(Item item)
    {
        ArgumentNullException.ThrowIfNull(item);

        _items.Add(item);

        AddDomainEvent(new CharacteristicsUpdatedDomainEvent(Id, _items));
    }

    public void RemoveItem(Item item)
    {
        ArgumentNullException.ThrowIfNull(item);

        _items.Remove(item);

        AddDomainEvent(new CharacteristicsUpdatedDomainEvent(Id, _items));
    }

    public void ReplaceItems(IEnumerable<Item> items)
    {
        ArgumentNullException.ThrowIfNull(items);

        if (!items.Any())
        {
            throw new ArgumentException("Items must be more then 0");
        }

        _items.Clear();
        _items.AddRange(items);

        AddDomainEvent(new CharacteristicsUpdatedDomainEvent(Id, _items));
    }

    public void ReplaceItem(Item oldItem, Item newItem)
    {
        ArgumentNullException.ThrowIfNull(oldItem);
        ArgumentNullException.ThrowIfNull(newItem);

        var index = _items.IndexOf(oldItem);
        if (index == -1)
            throw new InvalidGameOperationException("Item to replace not found");

        _items[index] = newItem;

        AddDomainEvent(new CharacteristicsUpdatedDomainEvent(Id, _items));
    }

    public void AddCard(Card card)
    {
        ArgumentNullException.ThrowIfNull(card);

        _cards.Add(card);

        AddDomainEvent(new CharacteristicsUpdatedDomainEvent(Id, _cards));
    }

    public void RemoveCard(Card card)
    {
        ArgumentNullException.ThrowIfNull(card);

        _cards.Remove(card);

        AddDomainEvent(new CharacteristicsUpdatedDomainEvent(Id, _cards));
    }

    public void ReplaceCards(IEnumerable<Card> cards)
    {
        ArgumentNullException.ThrowIfNull(cards);

        _cards.Clear();
        _cards.AddRange(cards);

        AddDomainEvent(new CharacteristicsUpdatedDomainEvent(Id, _cards));
    }

    public void ReplaceCard(Card oldCard, Card newCard)
    {
        ArgumentNullException.ThrowIfNull(oldCard);
        ArgumentNullException.ThrowIfNull(newCard);

        var index = _cards.IndexOf(oldCard);
        if (index == -1)
            throw new InvalidGameOperationException("Trait to replace not found");

        _cards[index] = newCard;

        AddDomainEvent(new CharacteristicsUpdatedDomainEvent(Id, _cards));
    }

    public CardActionRequirements GetRequirements(Guid cardId)
    {
        var card = _cards.FirstOrDefault(x => x.Id == cardId) ?? throw new ArgumentException("Unknown card");

        return card.CardAction.GetCurrentCardActionRequirements();
    }

    public CardActionCommand UseCard(Guid cardId, ActivateCardParams activateCardParams)
    {
        if (IsKicked)
        {
            throw new InvalidGameOperationException("Character was kicked");
        }

        var card = _cards.FirstOrDefault(x => x.Id == cardId) ?? throw new ArgumentException("Unknown card");

        var command = card.ActivateCard(activateCardParams, GameSessionId);

        AddDomainEvent(new CardActivatedDomainEvent(GameSessionId, Id, card.Description));

        return command;
    }

    public void AddTrait(Trait trait)
    {
        ArgumentNullException.ThrowIfNull(trait);

        _traits.Add(trait);

        AddDomainEvent(new CharacteristicsUpdatedDomainEvent(Id, _traits));
    }

    public void RemoveTrait(Trait trait)
    {
        ArgumentNullException.ThrowIfNull(trait);

        _traits.Remove(trait);

        AddDomainEvent(new CharacteristicsUpdatedDomainEvent(Id, _traits));
    }

    public void ReplaceTraits(IEnumerable<Trait> traits)
    {
        ArgumentNullException.ThrowIfNull(traits);

        if (!traits.Any())
        {
            throw new ArgumentException("Traits must be more then 0");
        }

        _traits.Clear();

        _traits.AddRange(traits);

        AddDomainEvent(new CharacteristicsUpdatedDomainEvent(Id, _traits));
    }

    public void ReplaceTrait(Trait oldTrait, Trait newTrait)
    {
        ArgumentNullException.ThrowIfNull(oldTrait);
        ArgumentNullException.ThrowIfNull(newTrait);

        var index = _traits.IndexOf(oldTrait);
        if (index == -1)
            throw new InvalidGameOperationException("Trait to replace not found");

        _traits[index] = newTrait;

        AddDomainEvent(new CharacteristicsUpdatedDomainEvent(Id, _traits));
    }

    // Можно улучшить! Путем реализации отметок в характеристика что было раскрыто уже
    public IEnumerable<ICharacteristic> RevealCharacteristics(CharacteristicType characteristicType)
    {
        IEnumerable<ICharacteristic> characteristicsToReveal = characteristicType switch
        {
            CharacteristicType.Phobia => [Phobia],
            CharacteristicType.Hobby => [Hobby],
            CharacteristicType.AdditionalInformation => [AdditionalInformation],
            CharacteristicType.Health => [Health],
            CharacteristicType.CharacterItem => Items,
            CharacteristicType.Profession => [Profession],
            CharacteristicType.Trait => Traits,
            CharacteristicType.Card => Cards,
            _ => throw new NotImplementedException("Unknown characteristic"),
        };

        AddDomainEvent(new CharacteristicRevealedDomainEvent(Id, GameSessionId, characteristicsToReveal));

        return characteristicsToReveal;
    }

#pragma warning disable CS8618
#pragma warning disable T0008
    private Character(Guid id)
#pragma warning restore T0008
        : base(id) { }
#pragma warning disable CS8618
}
