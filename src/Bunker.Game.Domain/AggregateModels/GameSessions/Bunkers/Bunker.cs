namespace Bunker.Game.Domain.AggregateModels.GameSessions.Bunkers;

public class Bunker : ValueObject
{
    protected override IEnumerable<object> GetEqualityComponents()
    {
        throw new NotImplementedException();
    }
}
