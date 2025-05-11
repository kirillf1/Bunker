using Bunker.Game.Domain.AggregateModels.Characters;
using Bunker.Game.Domain.AggregateModels.Characters.Cards;
using Bunker.Game.Domain.AggregateModels.Characters.Characteristics;

namespace Bunker.Game.Infrastructure.Generators.CharacterFactories;

public class CharacterFactory : ICharacterFactory
{
    private readonly ICharacteristicGenerator _characteristicGenerator;

    public CharacterFactory(ICharacteristicGenerator characteristicGenerator)
    {
        _characteristicGenerator =
            characteristicGenerator ?? throw new ArgumentNullException(nameof(characteristicGenerator));
    }

    public async Task<Character> CreateCharacter(Guid gameSessionId)
    {
        var additionalInformation = await _characteristicGenerator.GenerateCharacteristic<AdditionalInformation>();
        var age = await _characteristicGenerator.GenerateCharacteristic<Age>();
        var childbearing = await _characteristicGenerator.GenerateCharacteristic<Childbearing>();
        var health = await _characteristicGenerator.GenerateCharacteristic<Health>();
        var hobby = await _characteristicGenerator.GenerateCharacteristic<Hobby>();
        var phobia = await _characteristicGenerator.GenerateCharacteristic<Phobia>();
        var profession = await _characteristicGenerator.GenerateCharacteristic<Profession>();
        var sex = await _characteristicGenerator.GenerateCharacteristic<Sex>();
        var size = await _characteristicGenerator.GenerateCharacteristic<Size>();

        var itemsCount = Random.Shared.Next(Character.MIN_ITEMS_IN_START_GAME, Character.MAX_ITEMS_IN_START_GAME + 1);
        var items = await _characteristicGenerator.GenerateCharacteristics<Item>(itemsCount);

        var traitsCount = Random.Shared.Next(
            Character.MIN_TRAITS_IN_START_GAME,
            Character.MAX_TRAITS_IN_START_GAME + 1
        );
        var traits = await _characteristicGenerator.GenerateCharacteristics<Trait>(traitsCount);

        var cardsCount = Random.Shared.Next(Character.MIN_CARDS_IN_START_GAME, Character.MAX_CARDS_IN_START_GAME + 1);
        var cards = await _characteristicGenerator.GenerateCharacteristics<Card>(cardsCount);

        return new Character(
            Guid.NewGuid(),
            gameSessionId,
            additionalInformation,
            age,
            childbearing,
            health,
            hobby,
            phobia,
            profession,
            sex,
            size,
            items,
            traits,
            cards
        );
    }

    public async Task<IEnumerable<Character>> CreateCharacters(Guid gameSessionId, int count)
    {
        if (count <= 0)
            throw new ArgumentException("Count must be greater than 0", nameof(count));

        var characters = new List<Character>();
        for (var i = 0; i < count; i++)
        {
            var character = await CreateCharacter(gameSessionId);
            characters.Add(character);
        }

        return characters;
    }
}
