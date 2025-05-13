namespace Bunker.Domain.Shared.DomainEvents;

public abstract class DomainEventHandlerBase<T> : IDomainEventHandler<T>
    where T : IDomainEvent
{
    public abstract Task Handle(T domainEvent);

    public async Task Handle(IDomainEvent domainEvent)
    {
        if (domainEvent is not T)
            throw new ArgumentException("Invalid domain event");

        await Handle((T)domainEvent);
    }
}
