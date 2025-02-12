namespace Bunker.Game.Domain.AggregateModels.Bunkers.Events;

public record BunkerComponentsUpdatedDomainEvent(
    Guid BunkerId,
    Guid GameSessionId,
    IEnumerable<IBunkerComponent> BunkerComponents
) : IDomainEvent;
