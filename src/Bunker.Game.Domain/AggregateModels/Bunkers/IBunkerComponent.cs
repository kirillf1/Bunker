namespace Bunker.Game.Domain.AggregateModels.Bunkers;

public interface IBunkerComponent
{
    public bool IsHidden { get; }

    string GetDescription();
}

public interface IBunkerComponent<out T> : IBunkerComponent
    where T : class, IBunkerComponent
{
    T Reveal();
}
