using Bunker.Game.Domain.AggregateModels.Characters;
using Bunker.Game.Infrastructure.Generators.CharacterFactories;
using Bunker.Game.Infrastructure.Generators.CharacteristicGenerators;
using Bunker.Game.Tests.Fakes;

namespace Bunker.Game.Tests.UnitTests.Characters;

public class CharacterFactoryTests
{
    private readonly CharacterFactory _factory;

    public CharacterFactoryTests()
    {
        var client = new FakeCharacterComponentsClient();
        var generator = new CharacteristicGenerator(client);
        _factory = new CharacterFactory(generator);
    }

    [Fact]
    public async Task CreateCharacter_ReturnsCharacterWithValidCharacteristics()
    {
        // Arrange
        var gameSessionId = Guid.NewGuid();

        // Act
        var character = await _factory.CreateCharacter(gameSessionId);

        // Assert
        Assert.NotNull(character);
        Assert.NotEqual(Guid.Empty, character.Id);
        Assert.Equal(gameSessionId, character.GameSessionId);

        // Check single characteristics
        Assert.NotNull(character.AdditionalInformation);
        Assert.NotNull(character.Age);
        Assert.NotNull(character.Childbearing);
        Assert.NotNull(character.Health);
        Assert.NotNull(character.Hobby);
        Assert.NotNull(character.Phobia);
        Assert.NotNull(character.Profession);
        Assert.NotNull(character.Sex);

        // Check collections
        var itemsCount = character.Items.Count;
        Assert.InRange(itemsCount, Character.MIN_ITEMS_IN_START_GAME, Character.MAX_ITEMS_IN_START_GAME);

        var traitsCount = character.Traits.Count();
        Assert.InRange(traitsCount, Character.MIN_TRAITS_IN_START_GAME, Character.MAX_TRAITS_IN_START_GAME);

        var cardsCount = character.Cards.Count();
        Assert.InRange(cardsCount, Character.MIN_CARDS_IN_START_GAME, Character.MAX_CARDS_IN_START_GAME);
    }

    [Fact]
    public async Task CreateCharacters_ReturnsCorrectNumberOfCharacters()
    {
        // Arrange
        var gameSessionId = Guid.NewGuid();
        const int count = 3;

        // Act
        var characters = await _factory.CreateCharacters(gameSessionId, count);

        // Assert
        Assert.NotNull(characters);
        Assert.Equal(count, characters.Count());

        var characterIds = characters.Select(c => c.Id).ToList();
        Assert.Equal(count, characterIds.Distinct().Count());
        Assert.All(characters, c => Assert.Equal(gameSessionId, c.GameSessionId));

        foreach (var character in characters)
        {
            Assert.NotNull(character.AdditionalInformation);
            Assert.NotNull(character.Age);
            Assert.NotNull(character.Childbearing);
            Assert.NotNull(character.Health);
            Assert.NotNull(character.Hobby);
            Assert.NotNull(character.Phobia);
            Assert.NotNull(character.Profession);
            Assert.NotNull(character.Sex);

            Assert.InRange(
                character.Items.Count(),
                Character.MIN_ITEMS_IN_START_GAME,
                Character.MAX_ITEMS_IN_START_GAME
            );
            Assert.InRange(
                character.Traits.Count(),
                Character.MIN_TRAITS_IN_START_GAME,
                Character.MAX_TRAITS_IN_START_GAME
            );
            Assert.InRange(
                character.Cards.Count(),
                Character.MIN_CARDS_IN_START_GAME,
                Character.MAX_CARDS_IN_START_GAME
            );
        }
    }

    [Fact]
    public async Task CreateCharacters_ThrowsArgumentException_WhenCountIsZero()
    {
        // Arrange
        var gameSessionId = Guid.NewGuid();
        const int count = 0;

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => _factory.CreateCharacters(gameSessionId, count));
    }

    [Fact]
    public async Task CreateCharacters_ThrowsArgumentException_WhenCountIsNegative()
    {
        // Arrange
        var gameSessionId = Guid.NewGuid();
        const int count = -1;

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => _factory.CreateCharacters(gameSessionId, count));
    }
}
