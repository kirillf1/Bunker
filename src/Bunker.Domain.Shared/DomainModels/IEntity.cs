using Bunker.Domain.Shared.DomainEvents;

namespace Bunker.Domain.Shared.DomainModels;

public interface IEntity
{
    IReadOnlyCollection<IDomainEvent>? DomainEvents { get; }

    void AddDomainEvent(IDomainEvent eventItem);
    void ClearDomainEvents();
    void RemoveDomainEvent(IDomainEvent eventItem);
}
