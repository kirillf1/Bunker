using Bunker.Game.Domain.AggregateModels.Bunkers;
using Bunker.Game.Infrastructure.Http.GameComponents.Contracts;
using BunkerAggregate = Bunker.Game.Domain.AggregateModels.Bunkers.Bunker;
using Environment = Bunker.Game.Domain.AggregateModels.Bunkers.Environment;

namespace Bunker.Game.Infrastructure.Generators.BunkerGenerators;

public class BunkerGenerator : IBunkerGenerator
{
    private readonly IBunkerComponentsClient _client;

    public BunkerGenerator(IBunkerComponentsClient client)
    {
        _client = client ?? throw new ArgumentNullException(nameof(client));
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

        if (componentType == typeof(Item))
        {
            var itemsDto = await _client.ItemsGetAsync();
            if (itemsDto.Count == 0)
                throw new InvalidOperationException($"No {componentType.Name} data available.");
            var itemDto = itemsDto.ElementAt(Random.Shared.Next(0, itemsDto.Count));
            return new Item(itemDto.Description);
        }
        else if (componentType == typeof(Room))
        {
            var roomsDto = await _client.RoomsGetAsync();
            if (roomsDto.Count == 0)
                throw new InvalidOperationException($"No {componentType.Name} data available.");
            var roomDto = roomsDto.ElementAt(Random.Shared.Next(0, roomsDto.Count));
            return new Room(roomDto.Description);
        }
        else if (componentType == typeof(Environment))
        {
            var environmentsDto = await _client.EnvironmentsGetAsync();
            if (environmentsDto.Count == 0)
                throw new InvalidOperationException($"No {componentType.Name} data available.");
            var environmentDto = environmentsDto.ElementAt(Random.Shared.Next(0, environmentsDto.Count));
            return new Environment(environmentDto.Description);
        }

        throw new NotSupportedException($"Type {componentType.Name} is not supported as a bunker component.");
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
        return (T?)await GetBunkerComponent(id, typeof(T));
    }

    public async Task<IBunkerComponent?> GetBunkerComponent(Guid id, Type componentType)
    {
        if (!typeof(IBunkerComponent).IsAssignableFrom(componentType))
            throw new NotSupportedException($"Type {componentType.Name} does not implement IBunkerComponent.");

        try
        {
            if (componentType == typeof(Item))
            {
                var itemDto = await _client.ItemsGetAsync(id);
                return new Item(itemDto.Description);
            }
            else if (componentType == typeof(Room))
            {
                var roomDto = await _client.RoomsGetAsync(id);
                return new Room(roomDto.Description);
            }
            else if (componentType == typeof(Environment))
            {
                var environmentDto = await _client.EnvironmentsGetAsync(id);
                return new Environment(environmentDto.Description);
            }

            throw new NotSupportedException($"Type {componentType.Name} is not supported as a bunker component.");
        }
        catch (ApiException ex) when (ex.StatusCode == 404)
        {
            return null;
        }
    }

    public async Task<BunkerAggregate> GenerateBunker(Guid gameSessionId)
    {
        var rooms = await GenerateBunkerComponents<Room>(
            Random.Shared.Next(BunkerAggregate.MIN_ROOMS_COUNT, BunkerAggregate.MAX_ROOMS_COUNT + 1)
        );
        var items = await GenerateBunkerComponents<Item>(
            Random.Shared.Next(BunkerAggregate.MIN_BUNKER_ITEM_COUNT, BunkerAggregate.MAX_BUNKER_ITEM_COUNT + 1)
        );
        var environment = await GenerateBunkerComponents<Environment>(
            Random.Shared.Next(
                BunkerAggregate.MIN_BUNKER_ENVIROMENT_COUNT,
                BunkerAggregate.MAX_BUNKER_ENVIROMENT_COUNT + 1
            )
        );
        var description = await GenerateBunkerDescription();

        return new BunkerAggregate(Guid.CreateVersion7(), gameSessionId, description, items, environment, rooms);
    }

    public async Task<string> GenerateBunkerDescription()
    {
        var descriptions = await _client.DescriptionsGetAsync();

        return descriptions.ElementAt(Random.Shared.Next(0, descriptions.Count)).Text;
    }
}
