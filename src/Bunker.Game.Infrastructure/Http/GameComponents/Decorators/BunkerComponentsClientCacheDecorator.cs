using Bunker.Game.Infrastructure.Http.GameComponents.Contracts;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;

namespace Bunker.Game.Infrastructure.Http.GameComponents.Decorators;

public class BunkerComponentsClientCacheDecorator : BaseCacheDecorator, IBunkerComponentsClient
{
    private readonly BunkerComponentsClient _innerClient;

    public BunkerComponentsClientCacheDecorator(
        BunkerComponentsClient innerClient,
        IDistributedCache cache,
        IOptions<GameComponentsClientOptions> options
    )
        : base(cache, options)
    {
        _innerClient = innerClient;
    }

    public Task<ICollection<BunkerDescriptionDto>> DescriptionsGetAsync() =>
        DescriptionsGetAsync(CancellationToken.None);

    public Task<ICollection<BunkerDescriptionDto>> DescriptionsGetAsync(CancellationToken cancellationToken) =>
        GetCollectionFromCacheOrSource(
            CacheKeys.BunkerDescriptions,
            () => _innerClient.DescriptionsGetAsync(cancellationToken));

    public Task<BunkerDescriptionDto> DescriptionsGetAsync(Guid id) => DescriptionsGetAsync(id, CancellationToken.None);

    public Task<BunkerDescriptionDto> DescriptionsGetAsync(Guid id, CancellationToken cancellationToken) =>
        GetFromCacheOrSource(
            CacheKeys.GetItemKey(CacheKeys.BunkerDescriptions, id),
            () => _innerClient.DescriptionsGetAsync(id, cancellationToken));

    public Task<ICollection<BunkerItemDto>> ItemsGetAsync() => ItemsGetAsync(CancellationToken.None);

    public Task<ICollection<BunkerItemDto>> ItemsGetAsync(CancellationToken cancellationToken) =>
        GetCollectionFromCacheOrSource(
            CacheKeys.BunkerItems,
            () => _innerClient.ItemsGetAsync(cancellationToken));

    public Task<BunkerItemDto> ItemsGetAsync(Guid id) => ItemsGetAsync(id, CancellationToken.None);

    public Task<BunkerItemDto> ItemsGetAsync(Guid id, CancellationToken cancellationToken) =>
        GetFromCacheOrSource(
            CacheKeys.GetItemKey(CacheKeys.BunkerItems, id),
            () => _innerClient.ItemsGetAsync(id, cancellationToken));

    public Task<ICollection<EnvironmentDto>> EnvironmentsGetAsync() => EnvironmentsGetAsync(CancellationToken.None);

    public Task<ICollection<EnvironmentDto>> EnvironmentsGetAsync(CancellationToken cancellationToken) =>
        GetCollectionFromCacheOrSource(
            CacheKeys.BunkerEnvironments,
            () => _innerClient.EnvironmentsGetAsync(cancellationToken));

    public Task<EnvironmentDto> EnvironmentsGetAsync(Guid id) => EnvironmentsGetAsync(id, CancellationToken.None);

    public Task<EnvironmentDto> EnvironmentsGetAsync(Guid id, CancellationToken cancellationToken) =>
        GetFromCacheOrSource(
            CacheKeys.GetItemKey(CacheKeys.BunkerEnvironments, id),
            () => _innerClient.EnvironmentsGetAsync(id, cancellationToken));

    public Task<ICollection<RoomDto>> RoomsGetAsync() => RoomsGetAsync(CancellationToken.None);

    public Task<ICollection<RoomDto>> RoomsGetAsync(CancellationToken cancellationToken) =>
        GetCollectionFromCacheOrSource(
            CacheKeys.BunkerRooms,
            () => _innerClient.RoomsGetAsync(cancellationToken));

    public Task<RoomDto> RoomsGetAsync(Guid id) => RoomsGetAsync(id, CancellationToken.None);

    public Task<RoomDto> RoomsGetAsync(Guid id, CancellationToken cancellationToken) =>
        GetFromCacheOrSource(
            CacheKeys.GetItemKey(CacheKeys.BunkerRooms, id),
            () => _innerClient.RoomsGetAsync(id, cancellationToken));

    // Проксируем методы изменения данных без кэширования
    public Task<BunkerDescriptionDto> DescriptionsPostAsync(CreateBunkerDescriptionDto body) =>
        _innerClient.DescriptionsPostAsync(body);

    public Task<BunkerDescriptionDto> DescriptionsPostAsync(
        CreateBunkerDescriptionDto body,
        CancellationToken cancellationToken
    ) => _innerClient.DescriptionsPostAsync(body, cancellationToken);

    public Task DescriptionsPutAsync(Guid id, UpdateBunkerDescriptionDto body) =>
        _innerClient.DescriptionsPutAsync(id, body);

    public Task DescriptionsPutAsync(Guid id, UpdateBunkerDescriptionDto body, CancellationToken cancellationToken) =>
        _innerClient.DescriptionsPutAsync(id, body, cancellationToken);

    public Task DescriptionsDeleteAsync(Guid id) => _innerClient.DescriptionsDeleteAsync(id);

    public Task DescriptionsDeleteAsync(Guid id, CancellationToken cancellationToken) =>
        _innerClient.DescriptionsDeleteAsync(id, cancellationToken);

    public Task<BunkerItemDto> ItemsPostAsync(CreateBunkerItemDto body) => _innerClient.ItemsPostAsync(body);

    public Task<BunkerItemDto> ItemsPostAsync(CreateBunkerItemDto body, CancellationToken cancellationToken) =>
        _innerClient.ItemsPostAsync(body, cancellationToken);

    public Task ItemsPutAsync(Guid id, UpdateBunkerItemDto body) => _innerClient.ItemsPutAsync(id, body);

    public Task ItemsPutAsync(Guid id, UpdateBunkerItemDto body, CancellationToken cancellationToken) =>
        _innerClient.ItemsPutAsync(id, body, cancellationToken);

    public Task ItemsDeleteAsync(Guid id) => _innerClient.ItemsDeleteAsync(id);

    public Task ItemsDeleteAsync(Guid id, CancellationToken cancellationToken) =>
        _innerClient.ItemsDeleteAsync(id, cancellationToken);

    public Task<EnvironmentDto> EnvironmentsPostAsync(CreateEnvironmentDto body) =>
        _innerClient.EnvironmentsPostAsync(body);

    public Task<EnvironmentDto> EnvironmentsPostAsync(CreateEnvironmentDto body, CancellationToken cancellationToken) =>
        _innerClient.EnvironmentsPostAsync(body, cancellationToken);

    public Task EnvironmentsPutAsync(Guid id, UpdateEnvironmentDto body) => _innerClient.EnvironmentsPutAsync(id, body);

    public Task EnvironmentsPutAsync(Guid id, UpdateEnvironmentDto body, CancellationToken cancellationToken) =>
        _innerClient.EnvironmentsPutAsync(id, body, cancellationToken);

    public Task EnvironmentsDeleteAsync(Guid id) => _innerClient.EnvironmentsDeleteAsync(id);

    public Task EnvironmentsDeleteAsync(Guid id, CancellationToken cancellationToken) =>
        _innerClient.EnvironmentsDeleteAsync(id, cancellationToken);

    public Task<RoomDto> RoomsPostAsync(CreateRoomDto body) => _innerClient.RoomsPostAsync(body);

    public Task<RoomDto> RoomsPostAsync(CreateRoomDto body, CancellationToken cancellationToken) =>
        _innerClient.RoomsPostAsync(body, cancellationToken);

    public Task RoomsPutAsync(Guid id, UpdateRoomDto body) => _innerClient.RoomsPutAsync(id, body);

    public Task RoomsPutAsync(Guid id, UpdateRoomDto body, CancellationToken cancellationToken) =>
        _innerClient.RoomsPutAsync(id, body, cancellationToken);

    public Task RoomsDeleteAsync(Guid id) => _innerClient.RoomsDeleteAsync(id);

    public Task RoomsDeleteAsync(Guid id, CancellationToken cancellationToken) =>
        _innerClient.RoomsDeleteAsync(id, cancellationToken);
}
