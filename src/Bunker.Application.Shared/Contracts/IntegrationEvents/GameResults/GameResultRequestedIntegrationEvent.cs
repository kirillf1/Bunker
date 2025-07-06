using Bunker.Application.Shared.Contracts.IntegrationEvents.GameResults.GameComponents;
using Bunker.MessageBus.Abstractions;

namespace Bunker.Application.Shared.Contracts.IntegrationEvents.GameResults;

public record GameResultRequestedIntegrationEvent : IntegrationEvent
{
    public required Guid GameSessionId { get; init; }
    public required BunkerData Bunker { get; init; }
    public required CatastropheData Catastrophe { get; init; }
    public required IEnumerable<CharacterData> Characters { get; init; }
}
