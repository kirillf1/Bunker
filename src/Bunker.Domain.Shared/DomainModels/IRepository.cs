namespace Bunker.Domain.Shared.DomainModels;

public interface IRepository<T>
    where T : IAggregateRoot
{
    IUnitOfWork UnitOfWork { get; }
}
