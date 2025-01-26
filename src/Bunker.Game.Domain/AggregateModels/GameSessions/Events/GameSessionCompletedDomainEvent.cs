namespace Bunker.Game.Domain.AggregateModels.GameSessions.Events;

public record GameSessionCompletedDomainEvent(Guid GameSessionId, CompleteReason CompleteReason) : IDomainEvent;

public enum CompleteReason
{
    NormalCompletion,
    ForcedTermination,
}
