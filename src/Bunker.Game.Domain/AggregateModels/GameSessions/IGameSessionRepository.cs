namespace Bunker.Game.Domain.AggregateModels.GameSessions;

public interface IGameSessionRepository : IRepository<GameSession>
{
    Task<GameSession?> GetGameSession(Guid id);
}
