namespace Bunker.Domain.Shared.DomainModels;

public interface IUnitOfWork : IDisposable
{
    Task<int> SaveChangesAsync(CancellationToken cancel = default);
}
