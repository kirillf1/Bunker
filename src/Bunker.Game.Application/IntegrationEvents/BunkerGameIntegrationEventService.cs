using Bunker.MessageBus.Abstractions;
using Bunker.MessageBus.Abstractions.IntegrationEventLogs;

namespace Bunker.Game.Application.IntegrationEvents;

public class BunkerGameIntegrationEventService : IBunkerGameIntegrationEventService
{
    private readonly IMessageBus _messageBus;
    private readonly IIntegrationEventLogService _eventLogService;
    private readonly ILogger<BunkerGameIntegrationEventService> _logger;

    public BunkerGameIntegrationEventService(
        IMessageBus messageBus,
        IIntegrationEventLogService eventLogService,
        ILogger<BunkerGameIntegrationEventService> logger
    )
    {
        _messageBus = messageBus;
        _eventLogService = eventLogService;
        _logger = logger;
    }

    public async Task PublishEventsThroughEventBusAsync(Guid transactionId)
    {
        var pendingLogEvents = await _eventLogService.RetrieveEventLogsPendingToPublishAsync(transactionId);

        foreach (var logEvent in pendingLogEvents)
        {
            _logger.LogInformation(
                "Publishing integration event: {IntegrationEventId} - ({@IntegrationEvent})",
                logEvent.EventId,
                logEvent.IntegrationEvent
            );

            try
            {
                await _eventLogService.MarkEventAsInProgressAsync(logEvent.EventId);
                await _messageBus.PublishAsync(logEvent.IntegrationEvent!);
                await _eventLogService.MarkEventAsPublishedAsync(logEvent.EventId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error publishing integration event: {IntegrationEventId}", logEvent.EventId);

                await _eventLogService.MarkEventAsFailedAsync(logEvent.EventId);
            }
        }
    }

    public async Task AddAndSaveEventAsync(IntegrationEvent integrationEvent)
    {
        _logger.LogInformation(
            "Enqueuing integration event {IntegrationEventId} to repository ({@IntegrationEvent})",
            integrationEvent.Id,
            integrationEvent
        );

        await _eventLogService.SaveEventAsync(integrationEvent);
    }
}
