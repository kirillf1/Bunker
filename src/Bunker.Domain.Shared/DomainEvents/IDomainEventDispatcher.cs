namespace Bunker.Domain.Shared.DomainEvents;

public interface IDomainEventDispatcher
{
    public Task Notify(IDomainEvent domainEvent, CancellationToken cancel = default);
}
