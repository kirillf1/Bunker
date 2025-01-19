using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Bunker.Domain.Shared.DomainEvents;

public class DomainEventDispatcher : IDomainEventDispatcher
{
    private readonly ILogger<DomainEventDispatcher> _logger;
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public DomainEventDispatcher(ILogger<DomainEventDispatcher> logger, IServiceScopeFactory serviceScopeFactory)
    {
        _logger = logger;
        _serviceScopeFactory = serviceScopeFactory;
    }

    public async Task Notify(IDomainEvent domainEvent, CancellationToken cancel = default)
    {
        var domainEventType = domainEvent.GetType();

        _logger.LogInformation("Start handling: {DomainEvent}", domainEventType.Name);

        await using var scope = _serviceScopeFactory.CreateAsyncScope();

        var handlers = scope.ServiceProvider.GetKeyedServices<IDomainEventHandler>(domainEventType);

        foreach (var handler in handlers)
        {
            await handler.Handle(domainEvent);
        }
    }
}
