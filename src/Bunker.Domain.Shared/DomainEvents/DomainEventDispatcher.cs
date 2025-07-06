using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Bunker.Domain.Shared.DomainEvents;

public class DomainEventDispatcher : IDomainEventDispatcher
{
    private readonly ILogger<DomainEventDispatcher> _logger;
    private readonly IServiceProvider _serviceProvider;

    public DomainEventDispatcher(ILogger<DomainEventDispatcher> logger, IServiceProvider serviceProvider)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
    }

    public async Task Notify(IDomainEvent domainEvent, CancellationToken cancel = default)
    {
        var domainEventType = domainEvent.GetType();

        _logger.LogInformation("Start handling: {DomainEvent}", domainEventType.Name);

        var handlers = _serviceProvider.GetKeyedServices<IDomainEventHandler>(domainEventType);

        foreach (var handler in handlers)
        {
            await handler.Handle(domainEvent, cancel);
        }
    }
}
