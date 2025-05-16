namespace Bunker.Domain.Shared.DomainEvents;

public interface IDomainEventHandler<in T> : IDomainEventHandler
    where T : IDomainEvent
{
    Task Handle(T domainEvent, CancellationToken cancel);
}

public interface IDomainEventHandler
{
    Task Handle(IDomainEvent domainEvent, CancellationToken cancel);
}
