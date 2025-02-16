namespace Bunker.Game.Domain.AggregateModels.Bunkers;

public interface IBunkerGenerator
{
    Task<T> GenerateBunkerComponent<T>()
        where T : IBunkerComponent;

    Task<IEnumerable<T>> GenerateBunkerComponents<T>(int count)
        where T : IBunkerComponent;

    Task<T?> GetBunkerComponent<T>(Guid id)
        where T : IBunkerComponent;

    Task<Bunker> GenerateBunker(Guid gameSessionId);
}
