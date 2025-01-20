namespace Bunker.Game.Domain.AggregateModels.GameSessions.Characters;

public class Character : Entity<Guid>
{
    public Character(Guid id) : base(id)
    {
    }
}
