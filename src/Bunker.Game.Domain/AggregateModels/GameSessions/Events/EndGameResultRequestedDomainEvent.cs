namespace Bunker.Game.Domain.AggregateModels.GameSessions.Events;

public record EndGameResultRequestedDomainEvent(Guid GameSessionId, IEnumerable<Character> NotKickedCharacters) : IDomainEvent;
