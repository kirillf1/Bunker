namespace Bunker.Game.Domain.AggregateModels.Bunkers;

public interface IBunkerRepository : IRepository<Bunker>
{
    Task<Bunker?> GetBunker(Guid id);
    Task<Bunker?> GetBunkerByGameSessionId(Guid gameSessionId);
}
