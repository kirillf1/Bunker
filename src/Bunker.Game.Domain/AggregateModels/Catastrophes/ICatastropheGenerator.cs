namespace Bunker.Game.Domain.AggregateModels.Catastrophes;

public interface ICatastropheGenerator
{
    Task<string?> GetDescription(Guid id);

    Task<string> GenerateDescription();

    Task<Catastrophe> GenerateCatastrophe(Guid gameSessionId);
}
