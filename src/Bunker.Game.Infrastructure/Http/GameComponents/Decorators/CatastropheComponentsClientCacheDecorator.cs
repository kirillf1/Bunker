using Bunker.Game.Infrastructure.Http.GameComponents.Contracts;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace Bunker.Game.Infrastructure.Http.GameComponents.Decorators;

public class CatastropheComponentsClientCacheDecorator : BaseCacheDecorator, ICatastropheComponentsClient
{
    private readonly CatastropheComponentsClient _innerClient;

    public CatastropheComponentsClientCacheDecorator(
        CatastropheComponentsClient innerClient,
        IDistributedCache cache,
        IOptions<GameComponentsClientOptions> options) : base(cache, options)
    {
        _innerClient = innerClient;
    }

    public Task<ICollection<CatastropheDto>> DescriptionsGetAsync() =>
        DescriptionsGetAsync(CancellationToken.None);

    public Task<ICollection<CatastropheDto>> DescriptionsGetAsync(CancellationToken cancellationToken) =>
        GetCollectionFromCacheOrSource(
            CacheKeys.CatastropheDescriptions,
            () => _innerClient.DescriptionsGetAsync(cancellationToken));

    public Task<CatastropheDto> DescriptionsGetAsync(Guid id) =>
        DescriptionsGetAsync(id, CancellationToken.None);

    public Task<CatastropheDto> DescriptionsGetAsync(Guid id, CancellationToken cancellationToken) =>
        GetFromCacheOrSource(CacheKeys.GetItemKey(CacheKeys.CatastropheDescriptions, id),
            () => _innerClient.DescriptionsGetAsync(id, cancellationToken));

    // Проксируем методы изменения данных без кэширования
    public Task<CatastropheDto> DescriptionsPostAsync(CreateCatastropheDto body) =>
        _innerClient.DescriptionsPostAsync(body);

    public Task<CatastropheDto> DescriptionsPostAsync(CreateCatastropheDto body, CancellationToken cancellationToken) =>
        _innerClient.DescriptionsPostAsync(body, cancellationToken);

    public Task DescriptionsPutAsync(Guid id, UpdateCatastropheDto body) =>
        _innerClient.DescriptionsPutAsync(id, body);

    public Task DescriptionsPutAsync(Guid id, UpdateCatastropheDto body, CancellationToken cancellationToken) =>
        _innerClient.DescriptionsPutAsync(id, body, cancellationToken);

    public Task DescriptionsDeleteAsync(Guid id) =>
        _innerClient.DescriptionsDeleteAsync(id);

    public Task DescriptionsDeleteAsync(Guid id, CancellationToken cancellationToken) =>
        _innerClient.DescriptionsDeleteAsync(id, cancellationToken);
}
