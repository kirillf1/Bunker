namespace Bunker.Application.Shared.Contracts.IntegrationEvents.GameResults.GameComponents;

public record BunkerData(
    Guid Id,
    string Description,
    IEnumerable<BunkerRoomData> Rooms,
    IEnumerable<BunkerItemData> Items,
    IEnumerable<BunkerEnvironmentData> Environments
);
