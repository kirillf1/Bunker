namespace Bunker.Game.Domain.AggregateModels.Bunkers;

public interface IBunkerRepository : IRepository<BunkerAggregate>
{
    Task<BunkerAggregate?> GetBunker(Guid id);
    Task<BunkerAggregate?> GetBunkerByGameSessionId(Guid gameSessionId);
}
