using Bunker.MessageBus.Abstractions;

namespace Bunker.Application.Shared.Contracts.IntegrationEvents.GameResults;

public record GameResultRespondedIntegrationEvent : IntegrationEvent
{
    public required Guid GameSessionId { get; init; }
    public required string GameResultDescription { get; init; }
}
