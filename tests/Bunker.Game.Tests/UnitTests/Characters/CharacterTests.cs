using Bunker.Domain.Shared.GameComponents;
using Bunker.Game.Domain.AggregateModels.Characters;
using Bunker.Game.Domain.AggregateModels.Characters.Cards;
using Bunker.Game.Domain.AggregateModels.Characters.Cards.CardActions;
using Bunker.Game.Domain.AggregateModels.Characters.Characteristics;
using Bunker.Game.Domain.AggregateModels.Characters.Events;

namespace Bunker.Game.Tests.UnitTests.Characters;

public class CharacterTests
{
    private readonly Guid _id = Guid.NewGuid();
    private readonly Guid _gameSessionId = Guid.NewGuid();
    private readonly AdditionalInformation _additionalInformation;
    private readonly Age _age;
    private readonly Childbearing _childbearing;
    private readonly Health _health;
    private readonly Hobby _hobby;
    private readonly Phobia _phobia;
    private readonly Profession _profession;
    private readonly Sex _sex;
    private readonly Size _size;
    private readonly List<Item> _items;
    private readonly List<Trait> _traits;
    private readonly List<Card> _cards;

    public CharacterTests()
    {
        _additionalInformation = new AdditionalInformation("Test Info");
        _age = new Age(25);
        _childbearing = new Childbearing(true);
        _health = new Health(Guid.NewGuid().ToString());
        _hobby = new Hobby(Guid.NewGuid().ToString(), 1);
        _phobia = new Phobia(Guid.NewGuid().ToString());
        _profession = new Profession(Guid.NewGuid().ToString(), 1);
        _sex = new Sex(Guid.NewGuid().ToString());
        _size = new Size(170, 70);
        _items = new List<Item> { new(Guid.NewGuid().ToString()) };
        _traits = new List<Trait> { new(Guid.NewGuid().ToString()) };
        _cards = new List<Card>
        {
            new(Guid.NewGuid(), "Test Card", new EmptyAction(), Guid.NewGuid()),
            new(Guid.NewGuid(), "Test Card 2", new EmptyAction(), Guid.NewGuid()),
        };
    }

    [Fact]
    public void Constructor_ValidParameters_InitializesProperties()
    {
        // Arrange & Act
        var character = new Character(
            _id,
            _gameSessionId,
            _additionalInformation,
            _age,
            _childbearing,
            _health,
            _hobby,
            _phobia,
            _profession,
            _sex,
            _size,
            _items,
            _traits,
            _cards
        );

        // Assert
        Assert.Equal(_id, character.Id);
        Assert.Equal(_gameSessionId, character.GameSessionId);
        Assert.Equal(_additionalInformation, character.AdditionalInformation);
        Assert.Equal(_age, character.Age);
        Assert.Equal(_childbearing, character.Childbearing);
        Assert.Equal(_health, character.Health);
        Assert.Equal(_hobby, character.Hobby);
        Assert.Equal(_phobia, character.Phobia);
        Assert.Equal(_profession, character.Profession);
        Assert.Equal(_sex, character.Sex);
        Assert.Equal(_size, character.Size);
        Assert.False(character.IsKicked);
        Assert.Equal(_items, character.Items);
        Assert.Equal(_traits, character.Traits);
        Assert.Equal(_cards, character.Cards);
    }

    [Fact]
    public void Constructor_EmptyItems_ThrowsArgumentException()
    {
        // Arrange
        var emptyItems = new List<Item>();

        // Act & Assert
        Assert.Throws<ArgumentException>(
            () =>
                new Character(
                    _id,
                    _gameSessionId,
                    _additionalInformation,
                    _age,
                    _childbearing,
                    _health,
                    _hobby,
                    _phobia,
                    _profession,
                    _sex,
                    _size,
                    emptyItems,
                    _traits,
                    _cards
                )
        );
    }

    [Fact]
    public void Constructor_EmptyTraits_ThrowsArgumentException()
    {
        // Arrange
        var emptyTraits = new List<Trait>();

        // Act & Assert
        Assert.Throws<ArgumentException>(
            () =>
                new Character(
                    _id,
                    _gameSessionId,
                    _additionalInformation,
                    _age,
                    _childbearing,
                    _health,
                    _hobby,
                    _phobia,
                    _profession,
                    _sex,
                    _size,
                    _items,
                    emptyTraits,
                    _cards
                )
        );
    }

    [Fact]
    public void Constructor_InsufficientCards_ThrowsArgumentException()
    {
        // Arrange
        var insufficientCards = new List<Card> { new(Guid.NewGuid(), "Test Card", new EmptyAction(), Guid.NewGuid()) };

        // Act & Assert
        Assert.Throws<ArgumentException>(
            () =>
                new Character(
                    _id,
                    _gameSessionId,
                    _additionalInformation,
                    _age,
                    _childbearing,
                    _health,
                    _hobby,
                    _phobia,
                    _profession,
                    _sex,
                    _size,
                    _items,
                    _traits,
                    insufficientCards
                )
        );
    }

    [Fact]
    public void RecreateCharacter_ValidParameters_UpdatesPropertiesAndRaisesEvent()
    {
        // Arrange
        var character = new Character(
            _id,
            _gameSessionId,
            _additionalInformation,
            _age,
            _childbearing,
            _health,
            _hobby,
            _phobia,
            _profession,
            _sex,
            _size,
            _items,
            _traits,
            _cards
        );

        var newInfo = new AdditionalInformation("New Info");
        var newAge = new Age(30);
        var newChildbearing = new Childbearing(false);
        var newHealth = new Health("New Health");
        var newHobby = new Hobby("New Hobby", 2);
        var newPhobia = new Phobia("New Phobia");
        var newProfession = new Profession("New Profession", 2);
        var newSex = new Sex("New Sex");
        var newItems = new List<Item> { new("New Item") };
        var newTraits = new List<Trait> { new("New Trait") };
        var newCards = new List<Card>
        {
            new(Guid.NewGuid(), "New Card", new EmptyAction(), Guid.NewGuid()),
            new(Guid.NewGuid(), "New Card 2", new EmptyAction(), Guid.NewGuid()),
        };

        // Act
        character.RecreateCharacter(
            newInfo,
            newAge,
            newChildbearing,
            newHealth,
            newHobby,
            newPhobia,
            newProfession,
            newSex,
            newItems,
            newTraits,
            newCards
        );

        // Assert
        Assert.Equal(newInfo, character.AdditionalInformation);
        Assert.Equal(newAge, character.Age);
        Assert.Equal(newChildbearing, character.Childbearing);
        Assert.Equal(newHealth, character.Health);
        Assert.Equal(newHobby, character.Hobby);
        Assert.Equal(newPhobia, character.Phobia);
        Assert.Equal(newProfession, character.Profession);
        Assert.Equal(newSex, character.Sex);
        Assert.Equal(newItems, character.Items);
        Assert.Equal(newTraits, character.Traits);
        Assert.Equal(newCards, character.Cards);
        Assert.Contains(character.DomainEvents, e => e is CharacterRecreatedDomainEvent);
    }

    [Fact]
    public void UpdateAge_NewValue_UpdatesAgeAndRaisesEvent()
    {
        // Arrange
        var character = new Character(
            _id,
            _gameSessionId,
            _additionalInformation,
            _age,
            _childbearing,
            _health,
            _hobby,
            _phobia,
            _profession,
            _sex,
            _size,
            _items,
            _traits,
            _cards
        );
        var newAge = new Age(30);

        // Act
        character.UpdateAge(newAge);

        // Assert
        Assert.Equal(newAge, character.Age);
        Assert.Contains(character.DomainEvents, e => e is CharacteristicUpdatedDomainEvent);
    }

    [Fact]
    public void UpdateSize_NewValue_UpdatesSizeAndRaisesEvent()
    {
        // Arrange
        var character = new Character(
            _id,
            _gameSessionId,
            _additionalInformation,
            _age,
            _childbearing,
            _health,
            _hobby,
            _phobia,
            _profession,
            _sex,
            _size,
            _items,
            _traits,
            _cards
        );
        var newSize = new Size(180, 80);

        // Act
        character.UpdateSize(newSize);

        // Assert
        Assert.Equal(newSize, character.Size);
        Assert.Contains(character.DomainEvents, e => e is CharacteristicUpdatedDomainEvent);
    }

    [Fact]
    public void ReplaceItem_ValidItem_ReplacesItemAndRaisesEvent()
    {
        // Arrange
        var character = new Character(
            _id,
            _gameSessionId,
            _additionalInformation,
            _age,
            _childbearing,
            _health,
            _hobby,
            _phobia,
            _profession,
            _sex,
            _size,
            _items,
            _traits,
            _cards
        );
        var oldItem = _items[0];
        var newItem = new Item("New Item");

        // Act
        character.ReplaceItem(oldItem, newItem);

        // Assert
        Assert.DoesNotContain(oldItem, character.Items);
        Assert.Contains(newItem, character.Items);
        Assert.Contains(character.DomainEvents, e => e is CharacteristicsUpdatedDomainEvent);
    }

    [Fact]
    public void ReplaceCard_ValidCard_ReplacesCardAndRaisesEvent()
    {
        // Arrange
        var character = new Character(
            _id,
            _gameSessionId,
            _additionalInformation,
            _age,
            _childbearing,
            _health,
            _hobby,
            _phobia,
            _profession,
            _sex,
            _size,
            _items,
            _traits,
            _cards
        );
        var oldCard = _cards[0];
        var newCard = new Card(Guid.NewGuid(), "New Card", new EmptyAction(), Guid.NewGuid());

        // Act
        character.ReplaceCard(oldCard, newCard);

        // Assert
        Assert.DoesNotContain(oldCard, character.Cards);
        Assert.Contains(newCard, character.Cards);
        Assert.Contains(character.DomainEvents, e => e is CharacteristicsUpdatedDomainEvent);
    }

    [Fact]
    public void RevealCharacteristics_AgeType_ReturnsAgeAndRaisesEvent()
    {
        // Arrange
        var character = new Character(
            _id,
            _gameSessionId,
            _additionalInformation,
            _age,
            _childbearing,
            _health,
            _hobby,
            _phobia,
            _profession,
            _sex,
            _size,
            _items,
            _traits,
            _cards
        );

        // Act
        var characteristics = character.RevealCharacteristics(CharacteristicType.Age);

        // Assert
        Assert.Single(characteristics);
        Assert.Contains(_age, characteristics);
        Assert.Contains(character.DomainEvents, e => e is CharacteristicRevealedDomainEvent);
    }
}
