namespace Bunker.Game.Domain.AggregateModels.GameSessions;

public class Player : Entity<string>
{
    public string Name { get; private set; }

    public Player(string id, string name) : base(id)
    {
        Name = name;
    }

}
