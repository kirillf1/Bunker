namespace Bunker.Domain.Shared.DomainEvents;

public abstract class DomainEventHandlerBase<T> : IDomainEventHandler<T>
    where T : IDomainEvent
{
    public abstract Task Handle(T domainEvent, CancellationToken cancel);

    public async Task Handle(IDomainEvent domainEvent, CancellationToken cancel)
    {
        if (domainEvent is not T)
            throw new ArgumentException("Invalid domain event");

        await Handle((T)domainEvent, cancel);
    }
}
