using Bunker.Domain.Shared.DomainEvents;
using Bunker.Domain.Shared.DomainModels;

namespace Bunker.Game.Infrastructure.Data
{
    internal static class DomainEventsExtension
    {
        public static async Task DispatchDomainEvents(
            this IDomainEventDispatcher domainEventDispatcher,
            BunkerGameDbContext bunkerGameDbContext
        )
        {
            var domainEntities = bunkerGameDbContext
                .ChangeTracker.Entries<IEntity>()
                .Where(x => x.Entity.DomainEvents != null && x.Entity.DomainEvents.Count != 0)
                .Select(x => x.Entity);

            var domainEvents = domainEntities.SelectMany(x => x!.DomainEvents!).ToList();

            domainEntities.ToList().ForEach(entity => entity?.ClearDomainEvents());

            foreach (var domainEvent in domainEvents)
                await domainEventDispatcher.Notify(domainEvent);
        }
    }
}
