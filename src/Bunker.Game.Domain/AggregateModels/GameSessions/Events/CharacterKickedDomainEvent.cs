namespace Bunker.Game.Domain.AggregateModels.GameSessions.Events;

public record CharacterKickedDomainEvent(Guid GameSessionId, Guid CharacterId, string PlayerId) : IDomainEvent;
