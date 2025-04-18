using Bunker.Game.Infrastructure.Http.GameComponents.Contracts;

namespace Bunker.Game.Tests.Fakes;

public class FakeBunkerComponentsClient : IBunkerComponentsClient
{
    private static readonly Random _random = new();

    public Task<ICollection<BunkerDescriptionDto>> DescriptionsGetAsync() =>
        Task.FromResult<ICollection<BunkerDescriptionDto>>(GenerateDescriptions());

    public Task<ICollection<BunkerDescriptionDto>> DescriptionsGetAsync(CancellationToken cancellationToken) =>
        DescriptionsGetAsync();

    public Task<BunkerDescriptionDto> DescriptionsGetAsync(Guid id) => Task.FromResult(GenerateDescriptions().First());

    public Task<BunkerDescriptionDto> DescriptionsGetAsync(Guid id, CancellationToken cancellationToken) =>
        DescriptionsGetAsync(id);

    public Task<ICollection<BunkerItemDto>> ItemsGetAsync() =>
        Task.FromResult<ICollection<BunkerItemDto>>(GenerateItems());

    public Task<ICollection<BunkerItemDto>> ItemsGetAsync(CancellationToken cancellationToken) => ItemsGetAsync();

    public Task<BunkerItemDto> ItemsGetAsync(Guid id) => Task.FromResult(GenerateItems().First());

    public Task<BunkerItemDto> ItemsGetAsync(Guid id, CancellationToken cancellationToken) => ItemsGetAsync(id);

    public Task<ICollection<EnvironmentDto>> EnvironmentsGetAsync() =>
        Task.FromResult<ICollection<EnvironmentDto>>(GenerateEnvironments());

    public Task<ICollection<EnvironmentDto>> EnvironmentsGetAsync(CancellationToken cancellationToken) =>
        EnvironmentsGetAsync();

    public Task<EnvironmentDto> EnvironmentsGetAsync(Guid id) => Task.FromResult(GenerateEnvironments().First());

    public Task<EnvironmentDto> EnvironmentsGetAsync(Guid id, CancellationToken cancellationToken) =>
        EnvironmentsGetAsync(id);

    public Task<ICollection<RoomDto>> RoomsGetAsync() => Task.FromResult<ICollection<RoomDto>>(GenerateRooms());

    public Task<ICollection<RoomDto>> RoomsGetAsync(CancellationToken cancellationToken) => RoomsGetAsync();

    public Task<RoomDto> RoomsGetAsync(Guid id) => Task.FromResult(GenerateRooms().First());

    public Task<RoomDto> RoomsGetAsync(Guid id, CancellationToken cancellationToken) => RoomsGetAsync(id);

    // Not implemented methods throw by default
    public Task<BunkerDescriptionDto> DescriptionsPostAsync(CreateBunkerDescriptionDto body) =>
        throw new NotImplementedException();

    public Task<BunkerDescriptionDto> DescriptionsPostAsync(
        CreateBunkerDescriptionDto body,
        CancellationToken cancellationToken
    ) => throw new NotImplementedException();

    public Task DescriptionsPutAsync(Guid id, UpdateBunkerDescriptionDto body) => throw new NotImplementedException();

    public Task DescriptionsPutAsync(Guid id, UpdateBunkerDescriptionDto body, CancellationToken cancellationToken) =>
        throw new NotImplementedException();

    public Task DescriptionsDeleteAsync(Guid id) => throw new NotImplementedException();

    public Task DescriptionsDeleteAsync(Guid id, CancellationToken cancellationToken) =>
        throw new NotImplementedException();

    public Task<BunkerItemDto> ItemsPostAsync(CreateBunkerItemDto body) => throw new NotImplementedException();

    public Task<BunkerItemDto> ItemsPostAsync(CreateBunkerItemDto body, CancellationToken cancellationToken) =>
        throw new NotImplementedException();

    public Task ItemsPutAsync(Guid id, UpdateBunkerItemDto body) => throw new NotImplementedException();

    public Task ItemsPutAsync(Guid id, UpdateBunkerItemDto body, CancellationToken cancellationToken) =>
        throw new NotImplementedException();

    public Task ItemsDeleteAsync(Guid id) => throw new NotImplementedException();

    public Task ItemsDeleteAsync(Guid id, CancellationToken cancellationToken) => throw new NotImplementedException();

    public Task<EnvironmentDto> EnvironmentsPostAsync(CreateEnvironmentDto body) => throw new NotImplementedException();

    public Task<EnvironmentDto> EnvironmentsPostAsync(CreateEnvironmentDto body, CancellationToken cancellationToken) =>
        throw new NotImplementedException();

    public Task EnvironmentsPutAsync(Guid id, UpdateEnvironmentDto body) => throw new NotImplementedException();

    public Task EnvironmentsPutAsync(Guid id, UpdateEnvironmentDto body, CancellationToken cancellationToken) =>
        throw new NotImplementedException();

    public Task EnvironmentsDeleteAsync(Guid id) => throw new NotImplementedException();

    public Task EnvironmentsDeleteAsync(Guid id, CancellationToken cancellationToken) =>
        throw new NotImplementedException();

    public Task<RoomDto> RoomsPostAsync(CreateRoomDto body) => throw new NotImplementedException();

    public Task<RoomDto> RoomsPostAsync(CreateRoomDto body, CancellationToken cancellationToken) =>
        throw new NotImplementedException();

    public Task RoomsPutAsync(Guid id, UpdateRoomDto body) => throw new NotImplementedException();

    public Task RoomsPutAsync(Guid id, UpdateRoomDto body, CancellationToken cancellationToken) =>
        throw new NotImplementedException();

    public Task RoomsDeleteAsync(Guid id) => throw new NotImplementedException();

    public Task RoomsDeleteAsync(Guid id, CancellationToken cancellationToken) => throw new NotImplementedException();

    // Sample random data generators
    private List<BunkerDescriptionDto> GenerateDescriptions() =>
        Enumerable
            .Range(0, 5)
            .Select(_ => new BunkerDescriptionDto
            {
                Id = Guid.NewGuid(),
                Text = $"Some description text {_random.Next(1000)}",
            })
            .ToList();

    private List<BunkerItemDto> GenerateItems() =>
        Enumerable
            .Range(0, 10)
            .Select(_ => new BunkerItemDto { Id = Guid.NewGuid(), Description = $"Item {_random.Next(1000)}" })
            .ToList();

    private List<EnvironmentDto> GenerateEnvironments() =>
        Enumerable
            .Range(0, 10)
            .Select(_ => new EnvironmentDto { Id = Guid.NewGuid(), Description = $"Environment {_random.Next(1000)}" })
            .ToList();

    private List<RoomDto> GenerateRooms() =>
        Enumerable
            .Range(0, 10)
            .Select(_ => new RoomDto { Id = Guid.NewGuid(), Description = $"Room {_random.Next(1000)}" })
            .ToList();
}
