namespace Bunker.Game.Domain.AggregateModels.Characters;

public class Character : Entity<Guid>, IAggregateRoot
{
    public Character(Guid id)
        : base(id) { }
}
