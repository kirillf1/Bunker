using Bunker.Game.Infrastructure.Http.GameComponents.Contracts;

namespace Bunker.Game.Tests.Fakes
{
    internal class FakeCharacterComponentsClient : ICharacterComponentsClient
    {
        private static readonly IReadOnlyList<CardActionEntity> _cardActionEntities =
        [
            new EmptyActionEntity(),
            new RecreateCatastropheActionEntity(),
            new RecreateCharacterActionEntity() { TargetCharactersCount = 1 },
            new RecreateBunkerActionEntity(),
        ];

        public Task<ICollection<AdditionalInformationDto>> AdditionalInformationGetAsync()
        {
            var items = Enumerable
                .Range(0, 10)
                .Select(_ => new AdditionalInformationDto
                {
                    Id = Guid.NewGuid(),
                    Description = $"AdditionalInformation: {Random.Shared.Next()}",
                })
                .ToList();
            return Task.FromResult<ICollection<AdditionalInformationDto>>(items);
        }

        public Task<ICollection<AdditionalInformationDto>> AdditionalInformationGetAsync(
            CancellationToken cancellationToken
        )
        {
            cancellationToken.ThrowIfCancellationRequested();
            return AdditionalInformationGetAsync();
        }

        public Task<AdditionalInformationDto> AdditionalInformationGetAsync(Guid id)
        {
            return Task.FromResult(
                new AdditionalInformationDto { Id = id, Description = $"AdditionalInformation: {Random.Shared.Next()}" }
            );
        }

        public Task<AdditionalInformationDto> AdditionalInformationGetAsync(
            Guid id,
            CancellationToken cancellationToken
        )
        {
            cancellationToken.ThrowIfCancellationRequested();
            return AdditionalInformationGetAsync(id);
        }

        // Card Get methods
        public Task<ICollection<CardDto>> CardsGetAsync()
        {
            var items = Enumerable
                .Range(0, 10)
                .Select(_ => new CardDto
                {
                    Id = Guid.NewGuid(),
                    Description = $"Card: {Random.Shared.Next()}",
                    CardAction = _cardActionEntities[Random.Shared.Next(0, _cardActionEntities.Count)],
                })
                .ToList();
            return Task.FromResult<ICollection<CardDto>>(items);
        }

        public Task<ICollection<CardDto>> CardsGetAsync(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return CardsGetAsync();
        }

        public Task<CardDto> CardsGetAsync(Guid id)
        {
            return Task.FromResult(
                new CardDto
                {
                    Id = id,
                    Description = $"Card: {Random.Shared.Next()}",
                    CardAction = _cardActionEntities[Random.Shared.Next(0, _cardActionEntities.Count)],
                }
            );
        }

        public Task<CardDto> CardsGetAsync(Guid id, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return CardsGetAsync(id);
        }

        // Health Get methods
        public Task<ICollection<HealthDto>> HealthGetAsync()
        {
            var items = Enumerable
                .Range(0, 10)
                .Select(_ => new HealthDto { Id = Guid.NewGuid(), Description = $"Health: {Random.Shared.Next()}" })
                .ToList();
            return Task.FromResult<ICollection<HealthDto>>(items);
        }

        public Task<ICollection<HealthDto>> HealthGetAsync(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return HealthGetAsync();
        }

        public Task<HealthDto> HealthGetAsync(Guid id)
        {
            return Task.FromResult(new HealthDto { Id = id, Description = $"Health: {Random.Shared.Next()}" });
        }

        public Task<HealthDto> HealthGetAsync(Guid id, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return HealthGetAsync(id);
        }

        // Hobby Get methods
        public Task<ICollection<HobbyDto>> HobbiesGetAsync()
        {
            var items = Enumerable
                .Range(0, 10)
                .Select(_ => new HobbyDto { Id = Guid.NewGuid(), Description = $"Hobby: {Random.Shared.Next()}" })
                .ToList();
            return Task.FromResult<ICollection<HobbyDto>>(items);
        }

        public Task<ICollection<HobbyDto>> HobbiesGetAsync(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return HobbiesGetAsync();
        }

        public Task<HobbyDto> HobbiesGetAsync(Guid id)
        {
            return Task.FromResult(new HobbyDto { Id = id, Description = $"Hobby: {Random.Shared.Next()}" });
        }

        public Task<HobbyDto> HobbiesGetAsync(Guid id, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return HobbiesGetAsync(id);
        }

        // Item Get methods
        public Task<ICollection<ItemDto>> ItemsGetAsync()
        {
            var items = Enumerable
                .Range(0, 10)
                .Select(_ => new ItemDto { Id = Guid.NewGuid(), Description = $"Item: {Random.Shared.Next()}" })
                .ToList();
            return Task.FromResult<ICollection<ItemDto>>(items);
        }

        public Task<ICollection<ItemDto>> ItemsGetAsync(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return ItemsGetAsync();
        }

        public Task<ItemDto> ItemsGetAsync(Guid id)
        {
            return Task.FromResult(new ItemDto { Id = id, Description = $"Item: {Random.Shared.Next()}" });
        }

        public Task<ItemDto> ItemsGetAsync(Guid id, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return ItemsGetAsync(id);
        }

        // Phobia Get methods
        public Task<ICollection<PhobiaDto>> PhobiasGetAsync()
        {
            var items = Enumerable
                .Range(0, 10)
                .Select(_ => new PhobiaDto { Id = Guid.NewGuid(), Description = $"Phobia: {Random.Shared.Next()}" })
                .ToList();
            return Task.FromResult<ICollection<PhobiaDto>>(items);
        }

        public Task<ICollection<PhobiaDto>> PhobiasGetAsync(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return PhobiasGetAsync();
        }

        public Task<PhobiaDto> PhobiasGetAsync(Guid id)
        {
            return Task.FromResult(new PhobiaDto { Id = id, Description = $"Phobia: {Random.Shared.Next()}" });
        }

        public Task<PhobiaDto> PhobiasGetAsync(Guid id, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return PhobiasGetAsync(id);
        }

        // Profession Get methods
        public Task<ICollection<ProfessionDto>> ProfessionsGetAsync()
        {
            var items = Enumerable
                .Range(0, 10)
                .Select(_ => new ProfessionDto
                {
                    Id = Guid.NewGuid(),
                    Description = $"Profession: {Random.Shared.Next()}",
                })
                .ToList();
            return Task.FromResult<ICollection<ProfessionDto>>(items);
        }

        public Task<ICollection<ProfessionDto>> ProfessionsGetAsync(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return ProfessionsGetAsync();
        }

        public Task<ProfessionDto> ProfessionsGetAsync(Guid id)
        {
            return Task.FromResult(new ProfessionDto { Id = id, Description = $"Profession: {Random.Shared.Next()}" });
        }

        public Task<ProfessionDto> ProfessionsGetAsync(Guid id, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return ProfessionsGetAsync(id);
        }

        // Trait Get methods
        public Task<ICollection<TraitDto>> TraitsGetAsync()
        {
            var items = Enumerable
                .Range(0, 10)
                .Select(_ => new TraitDto { Id = Guid.NewGuid(), Description = $"Trait: {Random.Shared.Next()}" })
                .ToList();
            return Task.FromResult<ICollection<TraitDto>>(items);
        }

        public Task<ICollection<TraitDto>> TraitsGetAsync(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return TraitsGetAsync();
        }

        public Task<TraitDto> TraitsGetAsync(Guid id)
        {
            return Task.FromResult(new TraitDto { Id = id, Description = $"Trait: {Random.Shared.Next()}" });
        }

        public Task<TraitDto> TraitsGetAsync(Guid id, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return TraitsGetAsync(id);
        }

        // Not implemented methods (Post, Put, Delete)
        public Task<AdditionalInformationDto> AdditionalInformationPostAsync(CreateAdditionalInformationDto body)
        {
            throw new NotImplementedException();
        }

        public Task<AdditionalInformationDto> AdditionalInformationPostAsync(
            CreateAdditionalInformationDto body,
            CancellationToken cancellationToken
        )
        {
            throw new NotImplementedException();
        }

        public Task AdditionalInformationPutAsync(Guid id, UpdateAdditionalInformationDto body)
        {
            throw new NotImplementedException();
        }

        public Task AdditionalInformationPutAsync(
            Guid id,
            UpdateAdditionalInformationDto body,
            CancellationToken cancellationToken
        )
        {
            throw new NotImplementedException();
        }

        public Task AdditionalInformationDeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task AdditionalInformationDeleteAsync(Guid id, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<CardDto> CardsPostAsync(CardCreateDto body)
        {
            throw new NotImplementedException();
        }

        public Task<CardDto> CardsPostAsync(CardCreateDto body, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task CardsPutAsync(Guid id, CardUpdateDto body)
        {
            throw new NotImplementedException();
        }

        public Task CardsPutAsync(Guid id, CardUpdateDto body, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task CardsDeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task CardsDeleteAsync(Guid id, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<HealthDto> HealthPostAsync(HealthCreateDto body)
        {
            throw new NotImplementedException();
        }

        public Task<HealthDto> HealthPostAsync(HealthCreateDto body, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task HealthPutAsync(Guid id, HealthUpdateDto body)
        {
            throw new NotImplementedException();
        }

        public Task HealthPutAsync(Guid id, HealthUpdateDto body, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task HealthDeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task HealthDeleteAsync(Guid id, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<HobbyDto> HobbiesPostAsync(CreateHobbyDto body)
        {
            throw new NotImplementedException();
        }

        public Task<HobbyDto> HobbiesPostAsync(CreateHobbyDto body, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task HobbiesPutAsync(Guid id, UpdateHobbyDto body)
        {
            throw new NotImplementedException();
        }

        public Task HobbiesPutAsync(Guid id, UpdateHobbyDto body, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task HobbiesDeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task HobbiesDeleteAsync(Guid id, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<ItemDto> ItemsPostAsync(CreateItemDto body)
        {
            throw new NotImplementedException();
        }

        public Task<ItemDto> ItemsPostAsync(CreateItemDto body, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task ItemsPutAsync(Guid id, UpdateItemDto body)
        {
            throw new NotImplementedException();
        }

        public Task ItemsPutAsync(Guid id, UpdateItemDto body, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task ItemsDeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task ItemsDeleteAsync(Guid id, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<PhobiaDto> PhobiasPostAsync(CreatePhobiaDto body)
        {
            throw new NotImplementedException();
        }

        public Task<PhobiaDto> PhobiasPostAsync(CreatePhobiaDto body, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task PhobiasPutAsync(Guid id, UpdatePhobiaDto body)
        {
            throw new NotImplementedException();
        }

        public Task PhobiasPutAsync(Guid id, UpdatePhobiaDto body, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task PhobiasDeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task PhobiasDeleteAsync(Guid id, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<ProfessionDto> ProfessionsPostAsync(CreateProfessionDto body)
        {
            throw new NotImplementedException();
        }

        public Task<ProfessionDto> ProfessionsPostAsync(CreateProfessionDto body, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task ProfessionsPutAsync(Guid id, UpdateProfessionDto body)
        {
            throw new NotImplementedException();
        }

        public Task ProfessionsPutAsync(Guid id, UpdateProfessionDto body, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task ProfessionsDeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task ProfessionsDeleteAsync(Guid id, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<TraitDto> TraitsPostAsync(CreateTraitDto body)
        {
            throw new NotImplementedException();
        }

        public Task<TraitDto> TraitsPostAsync(CreateTraitDto body, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task TraitsPutAsync(Guid id, UpdateTraitDto body)
        {
            throw new NotImplementedException();
        }

        public Task TraitsPutAsync(Guid id, UpdateTraitDto body, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task TraitsDeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task TraitsDeleteAsync(Guid id, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
