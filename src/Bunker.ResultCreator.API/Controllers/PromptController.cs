using Bunker.ResultCreator.API.Domain.GameResultPrompts;
using Bunker.ResultCreator.API.Domain.SurvivalPredictor;
using Bunker.ResultCreator.API.SurvivalScenarioGenerators;
using Microsoft.AspNetCore.Mvc;

namespace Bunker.ResultCreator.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PromptController : ControllerBase
{
    private readonly IPromptStorage _promptStorage;
    private readonly ISurvivalScenarioGenerator _survivalScenarioGenerator;
    private readonly ILogger<PromptController> _logger;

    public PromptController(
        IPromptStorage promptStorage,
        ISurvivalScenarioGenerator survivalScenarioGenerator,
        ILogger<PromptController> logger)
    {
        _promptStorage = promptStorage;
        _survivalScenarioGenerator = survivalScenarioGenerator;
        _logger = logger;
    }

    [HttpGet("survival-scenario")]
    public async Task<ActionResult<SurvivalScenarioPrompt>> GetSurvivalScenarioPrompt()
    {
        try
        {
            var prompts = await _promptStorage.GetSurvivalScenarioPrompt();
            return Ok(prompts);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving survival scenario prompt");
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpPost("predict-reproduction")]
    public async Task<ActionResult<BunkerReproductionCapabilityResult>> PredictReproduction(
        [FromBody] GameAnalysisContext gameContext,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var result = await _survivalScenarioGenerator.PredictBunkerReproductionCapability(
                gameContext, 
                cancellationToken);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error predicting reproduction capability");
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpPost("predict-survival")]
    public async Task<ActionResult<BunkerSurvivalCapabilityResult>> PredictSurvival(
        [FromBody] GameAnalysisContext gameContext,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var result = await _survivalScenarioGenerator.PredictSurvivalCapabilityResult(
                gameContext, 
                cancellationToken);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error predicting survival capability");
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpPost("generate-history")]
    public async Task<ActionResult<BunkerLifeHistory>> GenerateHistory(
        [FromBody] GenerateHistoryRequest request,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var result = await _survivalScenarioGenerator.GenerateBunkerLifeHistory(
                request.GameContext,
                request.ReproductionCapability,
                request.SurvivalCapability,
                cancellationToken);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generating bunker life history");
            return StatusCode(500, "Internal server error");
        }
    }
}

public class GenerateHistoryRequest
{
    public GameAnalysisContext GameContext { get; set; } = null!;
    public BunkerReproductionCapabilityResult ReproductionCapability { get; set; } = null!;
    public BunkerSurvivalCapabilityResult SurvivalCapability { get; set; } = null!;
}
