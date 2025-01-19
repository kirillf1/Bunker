namespace Bunker.Domain.Shared.DomainEvents;

public interface IDomainEventHandler<in T> : IDomainEventHandler
    where T : IDomainEvent
{
    Task Handle(T domainEvent);
}

public interface IDomainEventHandler
{
    Task Handle(IDomainEvent domainEvent);
}
