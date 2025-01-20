using Bunker.Domain.Shared.DomainEvents;

namespace Bunker.Domain.Shared.DomainModels;

public abstract class Entity<T>
{
    int? _requestedHashCode;

    private T _baseId;

    private List<IDomainEvent>? _domainEvents;

    public T Id
    {
        get { return _baseId; }
        protected set { _baseId = value; }
    }

    public IReadOnlyCollection<IDomainEvent>? DomainEvents => _domainEvents?.AsReadOnly();

    protected Entity(T id)
    {
        _baseId = id;
    }

    public void AddDomainEvent(IDomainEvent eventItem)
    {
        _domainEvents = _domainEvents ?? new List<IDomainEvent>();
        _domainEvents.Add(eventItem);
    }

    public void RemoveDomainEvent(IDomainEvent eventItem)
    {
        _domainEvents?.Remove(eventItem);
    }

    public void ClearDomainEvents()
    {
        _domainEvents?.Clear();
    }

    public override bool Equals(object? obj)
    {
        if (obj is not Entity<T>)
            return false;

        if (ReferenceEquals(this, obj))
            return true;

        if (GetType() != obj.GetType())
            return false;

        var item = (Entity<T>)obj;

        return item.Id!.Equals(Id);
    }


#pragma warning disable S2328
    public override int GetHashCode()
#pragma warning restore S2328
    {
        if (!_requestedHashCode.HasValue)
            _requestedHashCode = Id!.GetHashCode() ^ 31;

        return _requestedHashCode.Value;
    }
}
