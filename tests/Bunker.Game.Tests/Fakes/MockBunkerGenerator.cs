using Bogus;
using Bunker.Game.Domain.AggregateModels.Bunkers;
using Environment = Bunker.Game.Domain.AggregateModels.Bunkers.Environment;

namespace Bunker.Game.Tests.Fakes;

public class MockBunkerGenerator : IBunkerGenerator
{
    private readonly Faker _faker;
    private readonly Dictionary<Guid, IBunkerComponent> _componentCache = new();

    public MockBunkerGenerator()
    {
        _faker = new Faker("ru");
    }

    public async Task<T> GenerateBunkerComponent<T>()
        where T : IBunkerComponent
    {
        return (T)await GenerateBunkerComponent(typeof(T));
    }

    public async Task<IBunkerComponent> GenerateBunkerComponent(Type componentType)
    {
        if (!typeof(IBunkerComponent).IsAssignableFrom(componentType))
            throw new NotSupportedException($"Type {componentType.Name} does not implement IBunkerComponent.");

        var component = CreateBunkerComponent(componentType);
        _componentCache[Guid.NewGuid()] = component;

        return await Task.FromResult(component);
    }

    public async Task<IEnumerable<T>> GenerateBunkerComponents<T>(int count)
        where T : IBunkerComponent
    {
        if (count < 0)
            throw new ArgumentException("Count cannot be negative.", nameof(count));

        var components = new List<T>();
        for (int i = 0; i < count; i++)
        {
            var component = await GenerateBunkerComponent<T>();
            components.Add(component);
        }

        return components;
    }

    public async Task<T?> GetBunkerComponent<T>(Guid id)
        where T : IBunkerComponent
    {
        if (_componentCache.TryGetValue(id, out var cachedComponent) && cachedComponent is T castedComponent)
            return castedComponent;

        var component = await GenerateBunkerComponent<T>();
        _componentCache[id] = component;

        return component;
    }

    public async Task<BunkerAggregate> GenerateBunker(Guid gameSessionId)
    {
        int roomCount = _faker.Random.Int(BunkerAggregate.MIN_ROOMS_COUNT, BunkerAggregate.MAX_ROOMS_COUNT);
        int itemCount = _faker.Random.Int(BunkerAggregate.MIN_BUNKER_ITEM_COUNT, BunkerAggregate.MAX_BUNKER_ITEM_COUNT);
        int environmentCount = _faker.Random.Int(
            BunkerAggregate.MIN_BUNKER_ENVIROMENT_COUNT,
            BunkerAggregate.MAX_BUNKER_ENVIROMENT_COUNT
        );

        var rooms = await GenerateBunkerComponents<Room>(roomCount);
        var items = await GenerateBunkerComponents<Item>(itemCount);
        var environments = await GenerateBunkerComponents<Environment>(environmentCount);
        string description = await GenerateBunkerDescription();

        return new BunkerAggregate(Guid.NewGuid(), gameSessionId, description, items, environments, rooms);
    }

    public async Task<string> GenerateBunkerDescription()
    {
        return await Task.FromResult(_faker.Lorem.Paragraph(3));
    }

    private IBunkerComponent CreateBunkerComponent(Type componentType)
    {
        if (componentType == typeof(Item))
        {
            return new Item(_faker.Commerce.ProductName());
        }
        else if (componentType == typeof(Room))
        {
            return new Room(
                _faker.PickRandom(
                    "Спальня",
                    "Кухня",
                    "Лаборатория",
                    "Медицинский кабинет",
                    "Склад",
                    "Генераторная",
                    "Оружейная"
                )
            );
        }
        else if (componentType == typeof(Environment))
        {
            return new Environment(
                _faker.PickRandom(
                    "Радиация",
                    "Токсичные отходы",
                    "Наводнение",
                    "Недостаток кислорода",
                    "Повышенная влажность",
                    "Высокая температура",
                    "Инфекционные заболевания"
                )
            );
        }

        throw new NotSupportedException($"Type {componentType.Name} is not supported as a bunker component.");
    }
}
