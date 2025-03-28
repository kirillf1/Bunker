namespace Bunker.Game.Domain.AggregateModels.Catastrophes;

public interface ICatastropheRepository : IRepository<Catastrophe>
{
    Task<Catastrophe?> GetCatastrophe(Guid id);
    Task<Catastrophe?> GetCatastropheByGameSession(Guid gameSessionId);
}
