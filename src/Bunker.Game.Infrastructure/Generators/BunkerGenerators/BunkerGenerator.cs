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
        if (typeof(T) == typeof(Item))
        {
            var itemsDto = await _client.ItemsGetAsync();
            var itemDto = itemsDto.ElementAt(Random.Shared.Next(0, itemsDto.Count));
            return (T)Convert.ChangeType(new Item(itemDto.Description), typeof(T));
        }
        else if (typeof(T) == typeof(Room))
        {
            var roomsDto = await _client.RoomsGetAsync();
            var roomDto = roomsDto.ElementAt(Random.Shared.Next(0, roomsDto.Count));
            return (T)Convert.ChangeType(new Room(roomDto.Description), typeof(T));
        }
        else if (typeof(T) == typeof(Environment))
        {
            var environmentsDto = await _client.EnvironmentsGetAsync();
            var environmentDto = environmentsDto.ElementAt(Random.Shared.Next(0, environmentsDto.Count));
            return (T)Convert.ChangeType(new Environment(environmentDto.Description), typeof(T));
        }

        throw new NotSupportedException($"Type {typeof(T).Name} is not supported as a bunker component.");
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
        try
        {
            if (typeof(T) == typeof(Item))
            {
                var itemDto = await _client.ItemsGetAsync(id);
                return (T)Convert.ChangeType(new Item(itemDto.Description), typeof(T));
            }
            else if (typeof(T) == typeof(Room))
            {
                var roomDto = await _client.RoomsGetAsync(id);
                return (T)Convert.ChangeType(new Room(roomDto.Description), typeof(T));
            }
            else if (typeof(T) == typeof(Environment))
            {
                var environmentDto = await _client.EnvironmentsGetAsync(id);
                return (T)Convert.ChangeType(new Environment(environmentDto.Description), typeof(T));
            }

            throw new NotSupportedException($"Type {typeof(T).Name} is not supported as a bunker component.");
        }
        catch (ApiException ex) when (ex.StatusCode == 404)
        {
            return default;
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
