using Bunker.Domain.Shared.DomainModels;

namespace Bunker.Game.Infrastructure.Data.Repositories;

public class RepositoryBase<T> : IRepository<T>
    where T : class, IAggregateRoot
{
    protected readonly BunkerGameDbContext _context;

    public RepositoryBase(BunkerGameDbContext context)
    {
        _context = context;
    }

    public IUnitOfWork UnitOfWork => _context;

    public async Task Add(T aggregate)
    {
        await _context.Set<T>().AddAsync(aggregate);
    }

    public Task Update(T aggregate)
    {
        _context.Set<T>().Update(aggregate);
        return Task.CompletedTask;
    }

    public Task Delete(T aggregate)
    {
        _context.Set<T>().Remove(aggregate);
        return Task.CompletedTask;
    }
}
