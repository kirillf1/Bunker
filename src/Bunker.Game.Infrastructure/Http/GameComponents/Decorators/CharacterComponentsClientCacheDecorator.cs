using System.Text.Json;
using Bunker.Game.Infrastructure.Http.GameComponents.Contracts;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;

namespace Bunker.Game.Infrastructure.Http.GameComponents.Decorators;

public class CharacterComponentsClientCacheDecorator : BaseCacheDecorator, ICharacterComponentsClient
{
    private readonly CharacterComponentsClient _innerClient;

    public CharacterComponentsClientCacheDecorator(
        CharacterComponentsClient innerClient,
        IDistributedCache cache,
        IOptions<GameComponentsClientOptions> options) : base(cache, options)
    {
        _innerClient = innerClient;
    }

    // Кэшируемые GET методы
    public Task<ICollection<AdditionalInformationDto>> AdditionalInformationGetAsync() =>
        AdditionalInformationGetAsync(CancellationToken.None);

    public Task<ICollection<AdditionalInformationDto>> AdditionalInformationGetAsync(CancellationToken cancellationToken) =>
        GetCollectionFromCacheOrSource(
            CacheKeys.CharacterAdditionalInformation,
            () => _innerClient.AdditionalInformationGetAsync(cancellationToken));

    public Task<AdditionalInformationDto> AdditionalInformationGetAsync(Guid id) =>
        AdditionalInformationGetAsync(id, CancellationToken.None);

    public Task<AdditionalInformationDto> AdditionalInformationGetAsync(Guid id, CancellationToken cancellationToken) =>
        GetFromCacheOrSource(CacheKeys.GetItemKey(CacheKeys.CharacterAdditionalInformation, id),
            () => _innerClient.AdditionalInformationGetAsync(id, cancellationToken));

    public Task<ICollection<CardDto>> CardsGetAsync() =>
        CardsGetAsync(CancellationToken.None);

    public Task<ICollection<CardDto>> CardsGetAsync(CancellationToken cancellationToken) =>
        GetCollectionFromCacheOrSource(
            CacheKeys.CharacterCards,
            () => _innerClient.CardsGetAsync(cancellationToken));

    public Task<CardDto> CardsGetAsync(Guid id) =>
        CardsGetAsync(id, CancellationToken.None);

    public Task<CardDto> CardsGetAsync(Guid id, CancellationToken cancellationToken) =>
        GetFromCacheOrSource(CacheKeys.GetItemKey(CacheKeys.CharacterCards, id),
            () => _innerClient.CardsGetAsync(id, cancellationToken));

    public Task<ICollection<HealthDto>> HealthGetAsync() =>
        HealthGetAsync(CancellationToken.None);

    public Task<ICollection<HealthDto>> HealthGetAsync(CancellationToken cancellationToken) =>
        GetCollectionFromCacheOrSource(
            CacheKeys.CharacterHealth,
            () => _innerClient.HealthGetAsync(cancellationToken));

    public Task<HealthDto> HealthGetAsync(Guid id) =>
        HealthGetAsync(id, CancellationToken.None);

    public Task<HealthDto> HealthGetAsync(Guid id, CancellationToken cancellationToken) =>
        GetFromCacheOrSource(CacheKeys.GetItemKey(CacheKeys.CharacterHealth, id),
            () => _innerClient.HealthGetAsync(id, cancellationToken));

    public Task<ICollection<HobbyDto>> HobbiesGetAsync() =>
        HobbiesGetAsync(CancellationToken.None);

    public Task<ICollection<HobbyDto>> HobbiesGetAsync(CancellationToken cancellationToken) =>
        GetCollectionFromCacheOrSource(
            CacheKeys.CharacterHobbies,
            () => _innerClient.HobbiesGetAsync(cancellationToken));

    public Task<HobbyDto> HobbiesGetAsync(Guid id) =>
        HobbiesGetAsync(id, CancellationToken.None);

    public Task<HobbyDto> HobbiesGetAsync(Guid id, CancellationToken cancellationToken) =>
        GetFromCacheOrSource(CacheKeys.GetItemKey(CacheKeys.CharacterHobbies, id),
            () => _innerClient.HobbiesGetAsync(id, cancellationToken));

    public Task<ICollection<ItemDto>> ItemsGetAsync() =>
        ItemsGetAsync(CancellationToken.None);

    public Task<ICollection<ItemDto>> ItemsGetAsync(CancellationToken cancellationToken) =>
        GetCollectionFromCacheOrSource(
            CacheKeys.CharacterItems,
            () => _innerClient.ItemsGetAsync(cancellationToken));

    public Task<ItemDto> ItemsGetAsync(Guid id) =>
        ItemsGetAsync(id, CancellationToken.None);

    public Task<ItemDto> ItemsGetAsync(Guid id, CancellationToken cancellationToken) =>
        GetFromCacheOrSource(CacheKeys.GetItemKey(CacheKeys.CharacterItems, id),
            () => _innerClient.ItemsGetAsync(id, cancellationToken));

    public Task<ICollection<PhobiaDto>> PhobiasGetAsync() =>
        PhobiasGetAsync(CancellationToken.None);

    public Task<ICollection<PhobiaDto>> PhobiasGetAsync(CancellationToken cancellationToken) =>
        GetCollectionFromCacheOrSource(
            CacheKeys.CharacterPhobias,
            () => _innerClient.PhobiasGetAsync(cancellationToken));

    public Task<PhobiaDto> PhobiasGetAsync(Guid id) =>
        PhobiasGetAsync(id, CancellationToken.None);

    public Task<PhobiaDto> PhobiasGetAsync(Guid id, CancellationToken cancellationToken) =>
        GetFromCacheOrSource(CacheKeys.GetItemKey(CacheKeys.CharacterPhobias, id),
            () => _innerClient.PhobiasGetAsync(id, cancellationToken));

    public Task<ICollection<ProfessionDto>> ProfessionsGetAsync() =>
        ProfessionsGetAsync(CancellationToken.None);

    public Task<ICollection<ProfessionDto>> ProfessionsGetAsync(CancellationToken cancellationToken) =>
        GetCollectionFromCacheOrSource(
            CacheKeys.CharacterProfessions,
            () => _innerClient.ProfessionsGetAsync(cancellationToken));

    public Task<ProfessionDto> ProfessionsGetAsync(Guid id) =>
        ProfessionsGetAsync(id, CancellationToken.None);

    public Task<ProfessionDto> ProfessionsGetAsync(Guid id, CancellationToken cancellationToken) =>
        GetFromCacheOrSource(CacheKeys.GetItemKey(CacheKeys.CharacterProfessions, id),
            () => _innerClient.ProfessionsGetAsync(id, cancellationToken));

    public Task<ICollection<TraitDto>> TraitsGetAsync() =>
        TraitsGetAsync(CancellationToken.None);

    public Task<ICollection<TraitDto>> TraitsGetAsync(CancellationToken cancellationToken) =>
        GetCollectionFromCacheOrSource(
            CacheKeys.CharacterTraits,
            () => _innerClient.TraitsGetAsync(cancellationToken));

    public Task<TraitDto> TraitsGetAsync(Guid id) =>
        TraitsGetAsync(id, CancellationToken.None);

    public Task<TraitDto> TraitsGetAsync(Guid id, CancellationToken cancellationToken) =>
        GetFromCacheOrSource(CacheKeys.GetItemKey(CacheKeys.CharacterTraits, id),
            () => _innerClient.TraitsGetAsync(id, cancellationToken));

    // Проксируем методы изменения данных без кэширования
    public Task<AdditionalInformationDto> AdditionalInformationPostAsync(CreateAdditionalInformationDto body) =>
        _innerClient.AdditionalInformationPostAsync(body);

    public Task<AdditionalInformationDto> AdditionalInformationPostAsync(CreateAdditionalInformationDto body, CancellationToken cancellationToken) =>
        _innerClient.AdditionalInformationPostAsync(body, cancellationToken);

    public Task AdditionalInformationPutAsync(Guid id, UpdateAdditionalInformationDto body) =>
        _innerClient.AdditionalInformationPutAsync(id, body);

    public Task AdditionalInformationPutAsync(Guid id, UpdateAdditionalInformationDto body, CancellationToken cancellationToken) =>
        _innerClient.AdditionalInformationPutAsync(id, body, cancellationToken);

    public Task AdditionalInformationDeleteAsync(Guid id) =>
        _innerClient.AdditionalInformationDeleteAsync(id);

    public Task AdditionalInformationDeleteAsync(Guid id, CancellationToken cancellationToken) =>
        _innerClient.AdditionalInformationDeleteAsync(id, cancellationToken);

    public Task<CardDto> CardsPostAsync(CardCreateDto body) =>
        _innerClient.CardsPostAsync(body);

    public Task<CardDto> CardsPostAsync(CardCreateDto body, CancellationToken cancellationToken) =>
        _innerClient.CardsPostAsync(body, cancellationToken);

    public Task CardsPutAsync(Guid id, CardUpdateDto body) =>
        _innerClient.CardsPutAsync(id, body);

    public Task CardsPutAsync(Guid id, CardUpdateDto body, CancellationToken cancellationToken) =>
        _innerClient.CardsPutAsync(id, body, cancellationToken);

    public Task CardsDeleteAsync(Guid id) =>
        _innerClient.CardsDeleteAsync(id);

    public Task CardsDeleteAsync(Guid id, CancellationToken cancellationToken) =>
        _innerClient.CardsDeleteAsync(id, cancellationToken);

    public Task<HealthDto> HealthPostAsync(HealthCreateDto body) =>
        _innerClient.HealthPostAsync(body);

    public Task<HealthDto> HealthPostAsync(HealthCreateDto body, CancellationToken cancellationToken) =>
        _innerClient.HealthPostAsync(body, cancellationToken);

    public Task HealthPutAsync(Guid id, HealthUpdateDto body) =>
        _innerClient.HealthPutAsync(id, body);

    public Task HealthPutAsync(Guid id, HealthUpdateDto body, CancellationToken cancellationToken) =>
        _innerClient.HealthPutAsync(id, body, cancellationToken);

    public Task HealthDeleteAsync(Guid id) =>
        _innerClient.HealthDeleteAsync(id);

    public Task HealthDeleteAsync(Guid id, CancellationToken cancellationToken) =>
        _innerClient.HealthDeleteAsync(id, cancellationToken);

    public Task<HobbyDto> HobbiesPostAsync(CreateHobbyDto body) =>
        _innerClient.HobbiesPostAsync(body);

    public Task<HobbyDto> HobbiesPostAsync(CreateHobbyDto body, CancellationToken cancellationToken) =>
        _innerClient.HobbiesPostAsync(body, cancellationToken);

    public Task HobbiesPutAsync(Guid id, UpdateHobbyDto body) =>
        _innerClient.HobbiesPutAsync(id, body);

    public Task HobbiesPutAsync(Guid id, UpdateHobbyDto body, CancellationToken cancellationToken) =>
        _innerClient.HobbiesPutAsync(id, body, cancellationToken);

    public Task HobbiesDeleteAsync(Guid id) =>
        _innerClient.HobbiesDeleteAsync(id);

    public Task HobbiesDeleteAsync(Guid id, CancellationToken cancellationToken) =>
        _innerClient.HobbiesDeleteAsync(id, cancellationToken);

    public Task<ItemDto> ItemsPostAsync(CreateItemDto body) =>
        _innerClient.ItemsPostAsync(body);

    public Task<ItemDto> ItemsPostAsync(CreateItemDto body, CancellationToken cancellationToken) =>
        _innerClient.ItemsPostAsync(body, cancellationToken);

    public Task ItemsPutAsync(Guid id, UpdateItemDto body) =>
        _innerClient.ItemsPutAsync(id, body);

    public Task ItemsPutAsync(Guid id, UpdateItemDto body, CancellationToken cancellationToken) =>
        _innerClient.ItemsPutAsync(id, body, cancellationToken);

    public Task ItemsDeleteAsync(Guid id) =>
        _innerClient.ItemsDeleteAsync(id);

    public Task ItemsDeleteAsync(Guid id, CancellationToken cancellationToken) =>
        _innerClient.ItemsDeleteAsync(id, cancellationToken);

    public Task<PhobiaDto> PhobiasPostAsync(CreatePhobiaDto body) =>
        _innerClient.PhobiasPostAsync(body);

    public Task<PhobiaDto> PhobiasPostAsync(CreatePhobiaDto body, CancellationToken cancellationToken) =>
        _innerClient.PhobiasPostAsync(body, cancellationToken);

    public Task PhobiasPutAsync(Guid id, UpdatePhobiaDto body) =>
        _innerClient.PhobiasPutAsync(id, body);

    public Task PhobiasPutAsync(Guid id, UpdatePhobiaDto body, CancellationToken cancellationToken) =>
        _innerClient.PhobiasPutAsync(id, body, cancellationToken);

    public Task PhobiasDeleteAsync(Guid id) =>
        _innerClient.PhobiasDeleteAsync(id);

    public Task PhobiasDeleteAsync(Guid id, CancellationToken cancellationToken) =>
        _innerClient.PhobiasDeleteAsync(id, cancellationToken);

    public Task<ProfessionDto> ProfessionsPostAsync(CreateProfessionDto body) =>
        _innerClient.ProfessionsPostAsync(body);

    public Task<ProfessionDto> ProfessionsPostAsync(CreateProfessionDto body, CancellationToken cancellationToken) =>
        _innerClient.ProfessionsPostAsync(body, cancellationToken);

    public Task ProfessionsPutAsync(Guid id, UpdateProfessionDto body) =>
        _innerClient.ProfessionsPutAsync(id, body);

    public Task ProfessionsPutAsync(Guid id, UpdateProfessionDto body, CancellationToken cancellationToken) =>
        _innerClient.ProfessionsPutAsync(id, body, cancellationToken);

    public Task ProfessionsDeleteAsync(Guid id) =>
        _innerClient.ProfessionsDeleteAsync(id);

    public Task ProfessionsDeleteAsync(Guid id, CancellationToken cancellationToken) =>
        _innerClient.ProfessionsDeleteAsync(id, cancellationToken);

    public Task<TraitDto> TraitsPostAsync(CreateTraitDto body) =>
        _innerClient.TraitsPostAsync(body);

    public Task<TraitDto> TraitsPostAsync(CreateTraitDto body, CancellationToken cancellationToken) =>
        _innerClient.TraitsPostAsync(body, cancellationToken);

    public Task TraitsPutAsync(Guid id, UpdateTraitDto body) =>
        _innerClient.TraitsPutAsync(id, body);

    public Task TraitsPutAsync(Guid id, UpdateTraitDto body, CancellationToken cancellationToken) =>
        _innerClient.TraitsPutAsync(id, body, cancellationToken);

    public Task TraitsDeleteAsync(Guid id) =>
        _innerClient.TraitsDeleteAsync(id);

    public Task TraitsDeleteAsync(Guid id, CancellationToken cancellationToken) =>
        _innerClient.TraitsDeleteAsync(id, cancellationToken);
} 