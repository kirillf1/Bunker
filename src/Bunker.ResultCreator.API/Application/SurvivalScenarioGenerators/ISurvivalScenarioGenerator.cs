using Bunker.ResultCreator.API.Domain.SurvivalPredictor;

namespace Bunker.ResultCreator.API.Application.SurvivalScenarioGenerators;

public interface ISurvivalScenarioGenerator
{
    Task<BunkerReproductionCapabilityResult> PredictBunkerReproductionCapability(
        GameAnalysisContext gameContext,
        CancellationToken cancellationToken = default
    );

    Task<BunkerSurvivalCapabilityResult> PredictSurvivalCapabilityResult(
        GameAnalysisContext gameContext,
        CancellationToken cancellationToken = default
    );

    Task<BunkerLifeHistory> GenerateBunkerLifeHistory(
        GameAnalysisContext gameContext,
        BunkerReproductionCapabilityResult reproductionCapability,
        BunkerSurvivalCapabilityResult bunkerSurvivalCapability,
        CancellationToken cancellationToken = default
    );
}
