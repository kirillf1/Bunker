using Bunker.MessageBus.Abstractions;

namespace Bunker.Game.Application.IntegrationEvents;

public interface IBunkerGameIntegrationEventService
{
    Task PublishEventsThroughEventBusAsync(Guid transactionId);
    Task AddAndSaveEventAsync(IntegrationEvent integrationEvent);
}
