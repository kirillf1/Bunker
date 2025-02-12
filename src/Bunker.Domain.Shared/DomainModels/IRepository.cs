namespace Bunker.Domain.Shared.DomainModels;

public interface IRepository<in T>
    where T : IAggregateRoot
{
    IUnitOfWork UnitOfWork { get; }

    Task Add(T aggregate);

    Task Update(T aggregate);

    Task Delete(T aggregate);
}
