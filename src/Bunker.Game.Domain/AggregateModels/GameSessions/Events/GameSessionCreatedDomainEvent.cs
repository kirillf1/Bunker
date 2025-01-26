namespace Bunker.Game.Domain.AggregateModels.GameSessions.Events;

public record GameSessionCreatedDomainEvent(Guid GameSessionId) : IDomainEvent;
