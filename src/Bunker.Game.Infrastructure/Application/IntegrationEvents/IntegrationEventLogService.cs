using System.Reflection;
using Bunker.Application.Shared.Contracts.IntegrationEvents.GameResults;
using Bunker.Game.Infrastructure.Data;
using Bunker.MessageBus.Abstractions;
using Bunker.MessageBus.Abstractions.IntegrationEventLogs;
using Microsoft.EntityFrameworkCore;

namespace Bunker.Game.Infrastructure.Application.IntegrationEvents;

public class IntegrationEventLogService : IIntegrationEventLogService
{
    private readonly BunkerGameDbContext _context;
    private readonly Type[] _eventTypes;

    public IntegrationEventLogService(BunkerGameDbContext context)
    {
        _context = context;
        _eventTypes = typeof(GameResultRequestedIntegrationEvent)
            .Assembly.GetTypes()
            .Where(t => t.Name.EndsWith(nameof(IntegrationEvent)))
            .ToArray();
    }

    public async Task<IEnumerable<IntegrationEventLogEntry>> RetrieveEventLogsPendingToPublishAsync(Guid transactionId)
    {
        var result = await _context
            .IntegrationEventLogEntries.Where(e =>
                e.TransactionId == transactionId && e.State == EventState.NotPublished
            )
            .OrderBy(o => o.CreationTime)
            .ToListAsync();

        return result.Select(e =>
            e.DeserializeJsonContent(_eventTypes.FirstOrDefault(t => t.Name == e.EventTypeShortName)!)
        );
    }

    public async Task SaveEventAsync(IntegrationEvent @event)
    {
        var transaction =
            _context.GetCurrentTransaction()
            ?? throw new InvalidOperationException(
                "Transaction is null. To save integration event should create transaction"
            );

        var eventLogEntry = new IntegrationEventLogEntry(@event, transaction.TransactionId);

        await _context.IntegrationEventLogEntries.AddAsync(eventLogEntry);

        await _context.SaveChangesAsync();
    }

    public async Task MarkEventAsPublishedAsync(Guid eventId)
    {
        await UpdateEventStatusAsync(eventId, EventState.Published);
    }

    public async Task MarkEventAsInProgressAsync(Guid eventId)
    {
        await UpdateEventStatusAsync(eventId, EventState.InProgress);
    }

    public async Task MarkEventAsFailedAsync(Guid eventId)
    {
        await UpdateEventStatusAsync(eventId, EventState.PublishedFailed);
    }

    private async Task UpdateEventStatusAsync(Guid eventId, EventState status)
    {
        var eventLogEntry = await _context.IntegrationEventLogEntries.FirstOrDefaultAsync(e => e.EventId == eventId);

        if (eventLogEntry != null)
        {
            eventLogEntry.State = status;

            if (status == EventState.InProgress)
                eventLogEntry.TimesSent++;

            await _context.SaveChangesAsync();
        }
    }
}
