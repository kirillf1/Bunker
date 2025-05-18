using Bogus;
using Bunker.Game.Domain.AggregateModels.Characters;
using Bunker.Game.Domain.AggregateModels.Characters.Cards;
using Bunker.Game.Domain.AggregateModels.Characters.Cards.CardActions;
using Bunker.Game.Domain.AggregateModels.Characters.Characteristics;

namespace Bunker.Game.Tests.Fakes;

public class MockCharacteristicGenerator : ICharacteristicGenerator
{
    private readonly Faker _faker;
    private readonly Dictionary<Guid, ICharacteristic> _characteristicsCache = new();

    public MockCharacteristicGenerator()
    {
        _faker = new Faker("ru");
    }

    public async Task<T> GenerateCharacteristic<T>()
        where T : ICharacteristic
    {
        return (T)await GenerateCharacteristic(typeof(T));
    }

    public async Task<ICharacteristic> GenerateCharacteristic(Type characteristicType)
    {
        if (!typeof(ICharacteristic).IsAssignableFrom(characteristicType))
            throw new NotSupportedException($"Type {characteristicType.Name} does not implement ICharacteristic.");

        var characteristic = CreateCharacteristic(characteristicType);
        _characteristicsCache[Guid.NewGuid()] = characteristic;

        return await Task.FromResult(characteristic);
    }

    public async Task<IEnumerable<T>> GenerateCharacteristics<T>(int count)
        where T : ICharacteristic
    {
        if (count < 0)
            throw new ArgumentException("Count cannot be negative.", nameof(count));

        var characteristics = new List<T>();
        for (int i = 0; i < count; i++)
        {
            var characteristic = await GenerateCharacteristic<T>();
            characteristics.Add(characteristic);
        }

        return characteristics;
    }

    public async Task<IEnumerable<ICharacteristic>> GenerateCharacteristics(int count, Type characteristicType)
    {
        if (count < 0)
            throw new ArgumentException("Count cannot be negative.", nameof(count));

        var characteristics = new List<ICharacteristic>();
        for (int i = 0; i < count; i++)
        {
            var characteristic = await GenerateCharacteristic(characteristicType);
            characteristics.Add(characteristic);
        }

        return characteristics;
    }

    public async Task<T?> GetCharacteristic<T>(Guid id)
        where T : ICharacteristic
    {
        return (T?)await GetCharacteristic(id, typeof(T));
    }

    public async Task<ICharacteristic?> GetCharacteristic(Guid id, Type characteristicType)
    {
        if (!typeof(ICharacteristic).IsAssignableFrom(characteristicType))
            throw new NotSupportedException($"Type {characteristicType.Name} does not implement ICharacteristic.");

        if (_characteristicsCache.TryGetValue(id, out var cachedCharacteristic))
        {
            if (characteristicType.IsInstanceOfType(cachedCharacteristic))
                return cachedCharacteristic;
        }

        var characteristic = CreateCharacteristic(characteristicType);
        _characteristicsCache[id] = characteristic;

        return await Task.FromResult(characteristic);
    }

    private ICharacteristic CreateCharacteristic(Type characteristicType)
    {
        if (characteristicType == typeof(AdditionalInformation))
        {
            return new AdditionalInformation(_faker.Lorem.Sentence());
        }
        else if (characteristicType == typeof(Health))
        {
            return new Health(
                _faker.PickRandom(
                    "Полностью здоров",
                    "Хроническая аллергия",
                    "Диабет",
                    "Астма",
                    "Сердечно-сосудистое заболевание"
                )
            );
        }
        else if (characteristicType == typeof(Item))
        {
            return new Item(_faker.Commerce.ProductName());
        }
        else if (characteristicType == typeof(Phobia))
        {
            return new Phobia(
                _faker.PickRandom(
                    "Акрофобия - боязнь высоты",
                    "Клаустрофобия - боязнь замкнутых пространств",
                    "Арахнофобия - боязнь пауков",
                    "Социофобия - боязнь общения",
                    "Нозофобия - боязнь заболеть"
                )
            );
        }
        else if (characteristicType == typeof(Profession))
        {
            var experienceYears = (byte)
                _faker.Random.Number(Profession.MIN_GAME_EXPERIENCE_YEARS, Profession.MAX_GAME_EXPERIENCE_YEARS);

            return new Profession(_faker.Name.JobTitle(), experienceYears);
        }
        else if (characteristicType == typeof(Trait))
        {
            return new Trait(
                _faker.PickRandom(
                    "Общительность",
                    "Упрямство",
                    "Логическое мышление",
                    "Лидерские качества",
                    "Выносливость"
                )
            );
        }
        else if (characteristicType == typeof(Age))
        {
            return new Age(_faker.Random.Number(Age.MIN_GAME_CHARACTER_YEARS, Age.MAX_GAME_CHARACTER_YEARS));
        }
        else if (characteristicType == typeof(Childbearing))
        {
            return new Childbearing(_faker.Random.Bool());
        }
        else if (characteristicType == typeof(Card))
        {
            var cardAction = _faker.Random.Bool() ? (CardAction)new EmptyAction() : new RecreateBunkerAction();

            return new Card(Guid.NewGuid(), _faker.Lorem.Sentence(), cardAction);
        }
        else if (characteristicType == typeof(Hobby))
        {
            var experienceYears = (byte)
                _faker.Random.Number(Hobby.MIN_GAME_EXPERIENCE_YEARS, Hobby.MAX_GAME_EXPERIENCE_YEARS);

            return new Hobby(_faker.Hacker.Phrase(), experienceYears);
        }
        else if (characteristicType == typeof(Sex))
        {
            return new Sex(_faker.Random.Bool() ? "Мужчина" : "Женщина");
        }
        else if (characteristicType == typeof(Size))
        {
            return new Size(
                _faker.Random.Number(Size.MIN_CHARACTER_HEIGHT, Size.MAX_CHARACTER_HEIGHT),
                _faker.Random.Number(Size.MIN_CHARACTER_WEIGHT, Size.MAX_CHARACTER_WEIGHT)
            );
        }

        throw new NotSupportedException($"Type {characteristicType.Name} is not supported as a characteristic.");
    }
}
