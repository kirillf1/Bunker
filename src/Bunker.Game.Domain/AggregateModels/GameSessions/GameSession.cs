namespace Bunker.Game.Domain.AggregateModels.GameSessions;

public class GameSession : Entity<Guid>, IAggregateRoot
{
    public GameSession(Guid id) : base(id)
    {
    }
}
