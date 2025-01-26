namespace Bunker.Game.Domain.AggregateModels.GameSessions.Events;

public record GameSessionStartedDomainEvent(Guid GameSessionId) : IDomainEvent;
