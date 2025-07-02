using Bunker.ResultCreator.API.Domain.SurvivalPredictor;

namespace Bunker.ResultCreator.API.Application.GameSessionResults;

public interface IGameSessionResultService
{
    Task<string> CreateGameResultDescription(
        GameAnalysisContext gameAnalysisContext,
        CancellationToken cancellationToken = default
    );
}
