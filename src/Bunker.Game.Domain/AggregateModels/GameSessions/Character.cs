namespace Bunker.Game.Domain.AggregateModels.GameSessions;

public class Character : Entity<Guid>
{
    public Player? Player { get; private set; }

    public bool IsOccupiedByPlayer
    {
        get => Player is not null;
    }

    public bool IsGameCreator { get; private set; }

    public bool IsKicked { get; private set; }

    public Character(Guid id)
        : base(id) { }

    public Character(Guid id, Player player)
        : base(id)
    {
        Player = player;
    }

    public void OccupyCharacter(Player player, bool isGameCreator = false)
    {
        Player = player;
        IsGameCreator = isGameCreator;
    }

    public void MarkKicked()
    {
        IsKicked = true;
    }
}
