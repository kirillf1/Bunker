namespace Bunker.Game.Domain.AggregateModels.Bunkers.Events;

public record BunkerComponentRevealedDomainEvent(Guid BunkerId, Guid GameSessionId, IBunkerComponent BunkerComponent)
    : IDomainEvent;
