using System.Data;
using System.Text.Json;
using System.Text.Json.Serialization;
using Bunker.Domain.Shared.DomainEvents;
using Bunker.Domain.Shared.DomainModels;
using Bunker.Game.Domain.AggregateModels.Catastrophes;
using Bunker.Game.Domain.AggregateModels.GameSessions;
using Bunker.Game.Infrastructure.Data.Configurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using BunkerAggregate = Bunker.Game.Domain.AggregateModels.Bunkers;

namespace Bunker.Game.Infrastructure.Data
{
    public class BunkerGameDbContext : DbContext, IUnitOfWork
    {
        private IDbContextTransaction? _currentTransaction;
        private readonly IDomainEventDispatcher _domainEventDispatcher;

        public DbSet<GameSession> GameSessions { get; set; }
        public DbSet<BunkerAggregate.Bunker> Bunkers { get; set; }
        public DbSet<Catastrophe> Catastrophes { get; set; }

        public bool HasActiveTransaction => _currentTransaction != null;

        public BunkerGameDbContext(
            DbContextOptions<BunkerGameDbContext> options,
            IDomainEventDispatcher domainEventDispatcher
        )
            : base(options)
        {
            _domainEventDispatcher = domainEventDispatcher;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(BunkerGameDbContext).Assembly);
        }

        public async Task<IDbContextTransaction?> BeginTransactionAsync()
        {
            if (_currentTransaction != null)
                return null;

            _currentTransaction = await Database.BeginTransactionAsync(IsolationLevel.ReadCommitted);

            return _currentTransaction;
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            await _domainEventDispatcher.DispatchDomainEvents(this);

            return await base.SaveChangesAsync(cancellationToken);
        }

        public async Task CommitTransactionAsync(IDbContextTransaction transaction)
        {
            if (transaction == null)
                throw new ArgumentNullException(nameof(transaction));
            if (transaction != _currentTransaction)
                throw new InvalidOperationException($"Transaction {transaction.TransactionId} is not current");

            try
            {
                await SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch
            {
                RollbackTransaction();
                throw;
            }
            finally
            {
                if (HasActiveTransaction)
                {
                    _currentTransaction.Dispose();
                    _currentTransaction = null;
                }
            }
        }

        public void RollbackTransaction()
        {
            try
            {
                _currentTransaction?.Rollback();
            }
            finally
            {
                if (HasActiveTransaction)
                {
                    _currentTransaction?.Dispose();
                    _currentTransaction = null;
                }
            }
        }
    }
}
