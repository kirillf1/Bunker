namespace Bunker.Game.Domain.AggregateModels.Catastrophes;

public interface ICatastropheGenerator
{
    Task<string?> GetDescription(Guid id);

    Task<string> GenerateDescription(Guid id);

    Task<Catastrophe> GenerateCatastrophe(Guid gameSessionId);
}
