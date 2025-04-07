using Bunker.Game.Domain.AggregateModels.Bunkers;
using Bunker.Game.Infrastructure.Http.GameComponents;
using Bunker.Game.Infrastructure.Http.GameComponents.Contracts;
using BunkerAggregate = Bunker.Game.Domain.AggregateModels.Bunkers.Bunker;
using Environment = Bunker.Game.Domain.AggregateModels.Bunkers.Environment;

namespace Bunker.Game.Infrastructure.Generators.BunkerGenerators
{
    // Я понял, что BunkerComponentsClient не подойдет, нужно еще иметь интерфейс чтобы можно было кэшом задекорировать
    public class BunkerGenerator : IBunkerGenerator
    {
        private readonly BunkerComponentsClient _client;

        public BunkerGenerator(BunkerComponentsClient client)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
        }

        public async Task<T> GenerateBunkerComponent<T>()
            where T : IBunkerComponent
        {
            // Определяем тип компонента на основе T и создаем соответствующий DTO
            if (typeof(T) == typeof(BunkerItemDto))
            {
                var createDto = new CreateBunkerItemDto { Description = $"Generated bunker item {Guid.NewGuid()}" };
                var result = await _client.ItemsGetAsync();
                return (T)(object)result;
            }
            else if (typeof(T) == typeof(RoomDto))
            {
                var createDto = new CreateRoomDto { Description = $"Generated bunker room {Guid.NewGuid()}" };
                var result = await _client.RoomsPostAsync(createDto); // Предполагаем, что метод существует
                return (T)(object)result;
            }
            else if (typeof(T) == typeof(EnvironmentDto))
            {
                var createDto = new CreateEnvironmentDto { Description = $"Generated environment {Guid.NewGuid()}" };
                var result = await _client.EnvironmentsPostAsync(createDto);
                return (T)(object)result;
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
                if (typeof(T) == typeof(BunkerItemDto))
                {
                    var result = await _client.ItemsGetAsync(id);
                    return (T)(object)result;
                }
                else if (typeof(T) == typeof(RoomDto))
                {
                    var result = await _client.RoomsGetAsync(id); // Предполагаем, что метод существует
                    return (T)(object)result;
                }
                else if (typeof(T) == typeof(EnvironmentDto))
                {
                    var result = await _client.EnvironmentsGetAsync(id);
                    return (T)(object)result;
                }

                throw new NotSupportedException($"Type {typeof(T).Name} is not supported as a bunker component.");
            }
            catch (ApiException ex) when (ex.StatusCode == 404)
            {
                return default; // Возвращаем null, если компонент не найден
            }
        }

        public async Task<BunkerAggregate> GenerateBunker(Guid gameSessionId)
        {
            // Генерируем случайное количество комнат, предметов и окружение
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
            // Простая генерация описания

            var desctiptions = await _client.DescriptionsGetAsync();

            return desctiptions.First().Text;
        }
    }
}
