namespace Bunker.Game.Domain.AggregateModels.Bunkers.Events;

public record BunkerRecreatedDomainEvent(
    Guid BunkerId,
    Guid GameSessionId,
    string Description,
    IEnumerable<Item> Items,
    IEnumerable<Environment> Environments,
    IEnumerable<Room> Rooms
) : IDomainEvent;
