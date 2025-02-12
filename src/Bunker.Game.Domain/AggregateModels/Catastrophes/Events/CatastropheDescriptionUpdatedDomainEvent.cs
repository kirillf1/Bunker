namespace Bunker.Game.Domain.AggregateModels.Catastrophes.Events;

public record CatastropheDescriptionUpdatedDomainEvent(Guid CatastropheId, Guid GameSessionId, string Description)
    : IDomainEvent;
