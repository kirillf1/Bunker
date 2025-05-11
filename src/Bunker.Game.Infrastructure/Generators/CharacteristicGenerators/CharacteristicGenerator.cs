using Bunker.Game.Domain.AggregateModels.Characters;
using Bunker.Game.Domain.AggregateModels.Characters.Cards;
using Bunker.Game.Domain.AggregateModels.Characters.Characteristics;
using Bunker.Game.Infrastructure.Http.GameComponents.Contracts;
using Bunker.Game.Infrastructure.Http.GameComponents.Converters;

namespace Bunker.Game.Infrastructure.Generators.CharacteristicGenerators;

public class CharacteristicGenerator : ICharacteristicGenerator
{
    private readonly ICharacterComponentsClient _client;

    public CharacteristicGenerator(ICharacterComponentsClient client)
    {
        _client = client ?? throw new ArgumentNullException(nameof(client));
    }

    public async Task<T> GenerateCharacteristic<T>()
        where T : ICharacteristic
    {
        return (T)await GenerateCharacteristic(typeof(T));
    }

    // TODO если логика будет расширяться, то лучше под каждый тип создать фабрику, а тут к примеру искать через Service Provider
    public async Task<ICharacteristic> GenerateCharacteristic(Type characteristicType)
    {
        if (!typeof(ICharacteristic).IsAssignableFrom(characteristicType))
            throw new NotSupportedException($"Type {characteristicType.Name} does not implement ICharacteristic.");

        if (characteristicType == typeof(AdditionalInformation))
        {
            var additionalInformationDto = await _client.AdditionalInformationGetAsync();
            if (additionalInformationDto.Count == 0)
                throw new InvalidOperationException($"No {characteristicType.Name} data available.");
            var randomAdditionalInformation = additionalInformationDto.ElementAt(
                Random.Shared.Next(0, additionalInformationDto.Count)
            );
            return new AdditionalInformation(randomAdditionalInformation.Description);
        }
        else if (characteristicType == typeof(Health))
        {
            var healthDto = await _client.HealthGetAsync();
            if (healthDto.Count == 0)
                throw new InvalidOperationException($"No {characteristicType.Name} data available.");
            var randomHealth = healthDto.ElementAt(Random.Shared.Next(0, healthDto.Count));
            return new Health(randomHealth.Description);
        }
        else if (characteristicType == typeof(Item))
        {
            var itemDto = await _client.ItemsGetAsync();
            if (itemDto.Count == 0)
                throw new InvalidOperationException($"No {characteristicType.Name} data available.");
            var randomItem = itemDto.ElementAt(Random.Shared.Next(0, itemDto.Count));
            return new Item(randomItem.Description);
        }
        else if (characteristicType == typeof(Phobia))
        {
            var phobiaDto = await _client.PhobiasGetAsync();
            if (phobiaDto.Count == 0)
                throw new InvalidOperationException($"No {characteristicType.Name} data available.");
            var randomPhobia = phobiaDto.ElementAt(Random.Shared.Next(0, phobiaDto.Count));
            return new Phobia(randomPhobia.Description);
        }
        else if (characteristicType == typeof(Profession))
        {
            var professionDto = await _client.ProfessionsGetAsync();
            if (professionDto.Count == 0)
                throw new InvalidOperationException($"No {characteristicType.Name} data available.");
            var randomProfession = professionDto.ElementAt(Random.Shared.Next(0, professionDto.Count));
            var experienceYears = (byte)
                Random.Shared.Next(Profession.MIN_GAME_EXPERIENCE_YEARS, Profession.MAX_GAME_EXPERIENCE_YEARS);
            return new Profession(randomProfession.Description, experienceYears);
        }
        else if (characteristicType == typeof(Trait))
        {
            var traitDto = await _client.TraitsGetAsync();
            if (traitDto.Count == 0)
                throw new InvalidOperationException($"No {characteristicType.Name} data available.");
            var randomTrait = traitDto.ElementAt(Random.Shared.Next(0, traitDto.Count));
            return new Trait(randomTrait.Description);
        }
        else if (characteristicType == typeof(Age))
        {
            return new Age(Random.Shared.Next(Age.MIN_GAME_CHARACTER_YEARS, Age.MAX_GAME_CHARACTER_YEARS + 1));
        }
        else if (characteristicType == typeof(Childbearing))
        {
            return new Childbearing(Random.Shared.NextDouble() >= 0.5);
        }
        else if (characteristicType == typeof(Card))
        {
            var cardsDto = await _client.CardsGetAsync();
            if (cardsDto.Count == 0)
                throw new InvalidOperationException($"No {characteristicType.Name} data available.");

            var randomCard = cardsDto.ElementAt(Random.Shared.Next(0, cardsDto.Count));
            return new Card(randomCard.Id, randomCard.Description, randomCard.CardAction.ToCardAction());
        }
        else if (characteristicType == typeof(Hobby))
        {
            var hobbiesDto = await _client.HobbiesGetAsync();
            var randomHobby = hobbiesDto.ElementAt(Random.Shared.Next(0, hobbiesDto.Count));
            return new Hobby(
                randomHobby.Description,
                (byte)Random.Shared.Next(Hobby.MIN_GAME_EXPERIENCE_YEARS, Hobby.MAX_GAME_EXPERIENCE_YEARS)
            );
        }
        else if (characteristicType == typeof(Sex))
        {
            return new Sex(Random.Shared.NextDouble() >= 0.5 ? "Мужчина" : "Женщина");
        }
        else if (characteristicType == typeof(Size))
        {
            return new Size(
                Random.Shared.Next(Size.MIN_CHARACTER_HEIGHT, Size.MAX_CHARACTER_HEIGHT + 1),
                Random.Shared.Next(Size.MIN_CHARACTER_WEIGHT, Size.MAX_CHARACTER_WEIGHT + 1)
            );
        }
        throw new NotSupportedException($"Type {characteristicType.Name} is not supported as a characteristic.");
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

        try
        {
            if (characteristicType == typeof(AdditionalInformation))
            {
                var additionalInformationDto = await _client.AdditionalInformationGetAsync(id);
                return new AdditionalInformation(additionalInformationDto.Description);
            }
            else if (characteristicType == typeof(Health))
            {
                var healthDto = await _client.HealthGetAsync(id);
                return new Health(healthDto.Description);
            }
            else if (characteristicType == typeof(Item))
            {
                var itemDto = await _client.ItemsGetAsync(id);
                return new Item(itemDto.Description);
            }
            else if (characteristicType == typeof(Phobia))
            {
                var phobiaDto = await _client.PhobiasGetAsync(id);
                return new Phobia(phobiaDto.Description);
            }
            else if (characteristicType == typeof(Profession))
            {
                var professionDto = await _client.ProfessionsGetAsync(id);
                var experienceYears = (byte)
                    Random.Shared.Next(Profession.MIN_GAME_EXPERIENCE_YEARS, Profession.MAX_GAME_EXPERIENCE_YEARS);
                return new Profession(professionDto.Description, experienceYears);
            }
            else if (characteristicType == typeof(Trait))
            {
                var traitDto = await _client.TraitsGetAsync(id);
                return new Trait(traitDto.Description);
            }
            else if (characteristicType == typeof(Age))
            {
                return new Age(Random.Shared.Next(Age.MIN_GAME_CHARACTER_YEARS, Age.MAX_GAME_CHARACTER_YEARS + 1));
            }
            else if (characteristicType == typeof(Childbearing))
            {
                return new Childbearing(Random.Shared.NextDouble() >= 0.5);
            }
            else if (characteristicType == typeof(Card))
            {
                var cardDto = await _client.CardsGetAsync(id);
                return new Card(cardDto.Id, cardDto.Description, cardDto.CardAction.ToCardAction());
            }
            else if (characteristicType == typeof(Hobby))
            {
                var hobbyDto = await _client.HobbiesGetAsync(id);
                return new Hobby(
                    hobbyDto.Description,
                    (byte)Random.Shared.Next(Hobby.MIN_GAME_EXPERIENCE_YEARS, Hobby.MAX_GAME_EXPERIENCE_YEARS)
                );
            }
            else if (characteristicType == typeof(Sex))
            {
                return new Sex(Random.Shared.NextDouble() >= 0.5 ? "Мужчина" : "Женщина");
            }
            else if (characteristicType == typeof(Size))
            {
                return new Size(
                    Random.Shared.Next(Size.MIN_CHARACTER_HEIGHT, Size.MAX_CHARACTER_HEIGHT + 1),
                    Random.Shared.Next(Size.MIN_CHARACTER_WEIGHT, Size.MAX_CHARACTER_WEIGHT + 1)
                );
            }

            throw new NotSupportedException($"Type {characteristicType.Name} is not supported as a characteristic.");
        }
        catch (ApiException ex) when (ex.StatusCode == 404)
        {
            return null;
        }
    }
}
