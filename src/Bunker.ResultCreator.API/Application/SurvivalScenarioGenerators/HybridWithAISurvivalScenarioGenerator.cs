using System.Text.Json;
using System.Text.RegularExpressions;
using Bunker.ResultCreator.API.Application.SurvivalScenarioGenerators;
using Bunker.ResultCreator.API.Domain.GameResultPrompts;
using Bunker.ResultCreator.API.Domain.SurvivalPredictor;
using Bunker.ResultCreator.API.Infrastructure.AI.Options;
using Bunker.ResultCreator.API.Infrastructure.Json;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.Options;
using ChatRole = Microsoft.Extensions.AI.ChatRole;

namespace Bunker.ResultCreator.API.SurvivalScenarioGenerators;

public partial class HybridWithAISurvivalScenarioGenerator : ISurvivalScenarioGenerator
{
    private const int MaxAgeGiveBirth = 45;

    private readonly IChatClient _chatClient;
    private readonly IPromptStorage _promptStorage;
    private readonly ILogger<HybridWithAISurvivalScenarioGenerator> _logger;
    private readonly ChatOptions _chatOptions;
    private readonly JsonSerializerOptions _jsonSerializerOptions;

    public HybridWithAISurvivalScenarioGenerator(
        IChatClient chatClient,
        IPromptStorage promptStorage,
        IOptions<AIProviderOptions> aiOptions,
        ILogger<HybridWithAISurvivalScenarioGenerator> logger
    )
    {
        var chatConfig = aiOptions.Value.Chat;
        _chatOptions = new ChatOptions()
        {
            Temperature = chatConfig.Temperature,
            MaxOutputTokens = chatConfig.MaxOutputTokens,
            TopK = chatConfig.TopK,
            TopP = chatConfig.TopP,
        };
        _chatClient = chatClient;
        _promptStorage = promptStorage;
        _logger = logger;
        _jsonSerializerOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            ReadCommentHandling = JsonCommentHandling.Skip,
            AllowTrailingCommas = true,
        };
    }

    public async Task<BunkerLifeHistory> GenerateBunkerLifeHistory(
        GameAnalysisContext gameContext,
        BunkerReproductionCapabilityResult reproductionCapability,
        BunkerSurvivalCapabilityResult bunkerSurvivalCapability,
        CancellationToken cancellationToken = default
    )
    {
        var prompts = await _promptStorage.GetSurvivalScenarioPrompt();

        var historyPrompt =
            (reproductionCapability.CanGiveBirth && bunkerSurvivalCapability.CanSurvive)
                ? prompts.PositiveBunkerLifeHistoryPrompt
                : prompts.NegativeBunkerLifeHistoryPrompt;

        var gameDescription = gameContext.ToString();

        var messages = new[]
        {
            new ChatMessage(ChatRole.System, [new TextContent(historyPrompt)]),
            new ChatMessage(ChatRole.User, [new TextContent(gameDescription)]),
        };

        var history = await _chatClient
            .GetStreamingResponseAsync(messages, _chatOptions, cancellationToken)
            .ToChatResponseAsync(cancellationToken);

        return new BunkerLifeHistory(history.ToString());
    }

    public Task<BunkerReproductionCapabilityResult> PredictBunkerReproductionCapability(
        GameAnalysisContext gameContext,
        CancellationToken cancellationToken = default
    )
    {
        var canGiveBirthMan = gameContext.Characters.FirstOrDefault(x =>
            x.CanGiveBirth && x.Age <= MaxAgeGiveBirth && x.GetSexType() == Domain.Characters.SexType.Man
        );
        var canGiveBirthWoman = gameContext.Characters.FirstOrDefault(x =>
            x.CanGiveBirth && x.Age <= MaxAgeGiveBirth && x.GetSexType() == Domain.Characters.SexType.Woman
        );

        if (canGiveBirthMan is not null && canGiveBirthWoman is not null)
        {
            var reason =
                $"Персонаж {canGiveBirthMan.Name} и {canGiveBirthWoman.Name} могут завести детей. Так как оба являются детородного возраста";
            return Task.FromResult(new BunkerReproductionCapabilityResult(true, reason));
        }

        return Task.FromResult(new BunkerReproductionCapabilityResult(false));
    }

    public async Task<BunkerSurvivalCapabilityResult> PredictSurvivalCapabilityResult(
        GameAnalysisContext gameContext,
        CancellationToken cancellationToken = default
    )
    {
        const int maxRetries = 3;
        var prompts = await _promptStorage.GetSurvivalScenarioPrompt();
        var gameDescription = gameContext.ToString();

        var messages = new[]
        {
            new ChatMessage(ChatRole.System, [new TextContent(prompts.SurvivalCapabilityReasonPrompt)]),
            new ChatMessage(ChatRole.User, [new TextContent(gameDescription)]),
        };

        for (int attempt = 1; attempt <= maxRetries; attempt++)
        {
            var result = await TryGetSurvivalCapabilityResult(messages, attempt, maxRetries, cancellationToken);

            if (result.IsSuccess)
            {
                return result.Value!;
            }

            if (!result.ShouldRetry)
            {
                break;
            }
        }

        return new BunkerSurvivalCapabilityResult(false);
    }

    private async Task<AIAttemptResult> TryGetSurvivalCapabilityResult(
        ChatMessage[] messages,
        int attempt,
        int maxRetries,
        CancellationToken cancellationToken
    )
    {
        var json = string.Empty;

        try
        {
            var chatResponse = await _chatClient
                .GetStreamingResponseAsync(messages, _chatOptions, cancellationToken)
                .ToChatResponseAsync(cancellationToken);

            json = RetriveJsonMessageFromAIChat(chatResponse);

            var result = JsonSerializer.Deserialize<BunkerSurvivalCapabilityResult>(json, _jsonSerializerOptions);

            if (result != null)
            {
                return AIAttemptResult.Success(result);
            }

            _logger.LogWarning("Deserialized result is null on attempt {Attempt}", attempt);
            return AIAttemptResult.RetryableFailure();
        }
        catch (JsonException jsonEx)
        {
            _logger.LogWarning(
                jsonEx,
                "JSON parsing failed on attempt {Attempt}/{MaxRetries}. Will retry if attempts remain. Invalid json: {InvalidJson}",
                attempt,
                maxRetries,
                json
            );

            if (attempt == maxRetries)
            {
                _logger.LogError(
                    jsonEx,
                    "All {MaxRetries} attempts failed to parse JSON. Returning negative survival capability.",
                    maxRetries
                );
            }

            return AIAttemptResult.RetryableFailure();
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Unexpected error on attempt {Attempt}/{MaxRetries}. Returning negative survival capability.",
                attempt,
                maxRetries
            );

            return AIAttemptResult.FatalFailure();
        }
    }

    private static string RetriveJsonMessageFromAIChat(ChatResponse chatResponse)
    {
        var chatResult = chatResponse.ToString();

        var jsonMatch = JsonRegex().Match(chatResult);

        // Chat can return invalid character for deserialization
        var json = JsonInvalidCharacterReplacer()
            .Replace(
                jsonMatch.Value,
                m =>
                {
                    var cleaned = m.Groups[1]
                        .Value.Replace("\\\\n", "\\n")
                        .Replace("\\\\r", "\\n")
                        .Replace("\\\\t", "\\t")
                        .Replace("\\\\\"", "\\\"")
                        .Replace("\\\\/", "/")
                        .Replace("\\", "\\\\")
                        .Replace("\"", "\\\"")
                        .Replace("\r\n", "\\n")
                        .Replace("\r", "\\n")
                        .Replace("\n", "\\n")
                        .Replace("\t", "\\t")
                        .TrimEnd('\\', '/');

                    return $"\"Reason\": \"{cleaned}\"";
                }
            );
        return json;
    }

    [GeneratedRegex(@"\{[\s\S]*?\}", RegexOptions.Singleline)]
    private static partial Regex JsonRegex();

    [GeneratedRegex("\"Reason\"\\s*:\\s*\"([^\"]*(?:\\\\.[^\"]*)*)\"")]
    private static partial Regex JsonInvalidCharacterReplacer();

    private readonly record struct AIAttemptResult(
        bool IsSuccess,
        bool ShouldRetry,
        BunkerSurvivalCapabilityResult? Value
    )
    {
        public static AIAttemptResult Success(BunkerSurvivalCapabilityResult result) => new(true, false, result);

        public static AIAttemptResult RetryableFailure() => new(false, true, null);

        public static AIAttemptResult FatalFailure() => new(false, false, null);
    }
}
