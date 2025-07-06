using System.Text;
using Ardalis.Result;
using Bunker.Application.Shared.CQRS;
using Bunker.ResultCreator.API.Application.SurvivalScenarioGenerators;
using Bunker.ResultCreator.API.Domain.SurvivalPredictor;

namespace Bunker.ResultCreator.API.Application.Commands.CreateGameResult;

public class CreateGameResultCommandHandler : ICommandHandler<CreateGameResultCommand, Result<string>>
{
    private readonly ISurvivalScenarioGenerator _survivalScenarioGenerator;
    private readonly ILogger<CreateGameResultCommandHandler> _logger;

    public CreateGameResultCommandHandler(
        ISurvivalScenarioGenerator survivalScenarioGenerator,
        ILogger<CreateGameResultCommandHandler> logger
    )
    {
        _survivalScenarioGenerator = survivalScenarioGenerator;
        _logger = logger;
    }

    public async Task<Result<string>> Handle(CreateGameResultCommand command, CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Processing CreateGameResultCommand for GameSession {GameSessionId}",
            command.GameSessionId
        );

        try
        {
            var gameAnalysisContext = new GameAnalysisContext(
                command.GameSessionId,
                command.Bunker,
                command.Catastrophe,
                command.Characters
            );

            var result = await CreateGameResultDescription(gameAnalysisContext, cancellationToken);

            _logger.LogInformation(
                "Successfully created game result for GameSession {GameSessionId}",
                command.GameSessionId
            );

            return Result.Success(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to create game result for GameSession {GameSessionId}", command.GameSessionId);
            return Result.Error("Failed to create game result");
        }
    }

    private async Task<string> CreateGameResultDescription(
        GameAnalysisContext gameAnalysisContext,
        CancellationToken cancellationToken = default
    )
    {
        _logger.LogInformation(
            "Starting game result description creation for GameSession {GameSessionId}",
            gameAnalysisContext.GameSessionId
        );

        try
        {
            var stringBuilder = new StringBuilder();

            var reproductionResult = await GenerateReproductionResult(
                gameAnalysisContext,
                stringBuilder,
                cancellationToken
            );

            var survivalResult = await GenerateSurvivalResult(gameAnalysisContext, stringBuilder, cancellationToken);

            await GenerateBunkerHistory(
                gameAnalysisContext,
                stringBuilder,
                reproductionResult,
                survivalResult,
                cancellationToken
            );

            _logger.LogInformation(
                "Successfully completed game result description creation for GameSession {GameSessionId}",
                gameAnalysisContext.GameSessionId
            );

            return stringBuilder.ToString();
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Failed to create game result description for GameSession {GameSessionId}. Error: {ErrorMessage}",
                gameAnalysisContext.GameSessionId,
                ex.Message
            );
            throw;
        }
    }

    private async Task GenerateBunkerHistory(
        GameAnalysisContext gameAnalysisContext,
        StringBuilder stringBuilder,
        BunkerReproductionCapabilityResult reproductionResult,
        BunkerSurvivalCapabilityResult survivalResult,
        CancellationToken cancellationToken
    )
    {
        _logger.LogDebug(
            "Generating bunker life history for GameSession {GameSessionId}",
            gameAnalysisContext.GameSessionId
        );

        var lifeHistory = await _survivalScenarioGenerator.GenerateBunkerLifeHistory(
            gameAnalysisContext,
            reproductionResult,
            survivalResult,
            cancellationToken
        );
        stringBuilder.AppendLine();
        stringBuilder.AppendLine("История выживания в бункере:");
        stringBuilder.AppendLine(lifeHistory.Text);

        _logger.LogDebug(
            "Successfully generated bunker life history for GameSession {GameSessionId}",
            gameAnalysisContext.GameSessionId
        );
    }

    private async Task<BunkerSurvivalCapabilityResult> GenerateSurvivalResult(
        GameAnalysisContext gameAnalysisContext,
        StringBuilder stringBuilder,
        CancellationToken cancellationToken
    )
    {
        _logger.LogDebug(
            "Predicting survival capability for GameSession {GameSessionId}",
            gameAnalysisContext.GameSessionId
        );

        var survivalResult = await _survivalScenarioGenerator.PredictSurvivalCapabilityResult(
            gameAnalysisContext,
            cancellationToken
        );
        stringBuilder.AppendLine();
        stringBuilder.AppendLine("Смогут ли выжить в бункере:");
        stringBuilder.AppendLine($"Ответ: {(survivalResult.CanSurvive ? "Да" : "Нет")}");
        if (survivalResult.Reason is not null)
            stringBuilder.AppendLine($"Причина: {survivalResult.Reason}");

        _logger.LogDebug(
            "Successfully predicted survival capability for GameSession {GameSessionId}. CanSurvive: {CanSurvive}",
            gameAnalysisContext.GameSessionId,
            survivalResult.CanSurvive
        );

        return survivalResult;
    }

    private async Task<BunkerReproductionCapabilityResult> GenerateReproductionResult(
        GameAnalysisContext gameAnalysisContext,
        StringBuilder stringBuilder,
        CancellationToken cancellationToken
    )
    {
        _logger.LogDebug(
            "Predicting reproduction capability for GameSession {GameSessionId}",
            gameAnalysisContext.GameSessionId
        );

        var reproductionResult = await _survivalScenarioGenerator.PredictBunkerReproductionCapability(
            gameAnalysisContext,
            cancellationToken
        );
        stringBuilder.AppendLine("Могут ли дать потомство в бункере:");
        stringBuilder.AppendLine($"Ответ: {(reproductionResult.CanGiveBirth ? "Да" : "Нет")}");
        if (reproductionResult.Reason is not null)
            stringBuilder.AppendLine($"Причина: {reproductionResult.Reason}");

        _logger.LogDebug(
            "Successfully predicted reproduction capability for GameSession {GameSessionId}. CanGiveBirth: {CanGiveBirth}",
            gameAnalysisContext.GameSessionId,
            reproductionResult.CanGiveBirth
        );

        return reproductionResult;
    }
}
