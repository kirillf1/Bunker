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
            Converters = { new AISurvivalCapabilityResponseJsonConverter() },
            PropertyNameCaseInsensitive = true,
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
        var prompts = await _promptStorage.GetSurvivalScenarioPrompt();
        var gameDescription = gameContext.ToString();

        var messages = new[]
        {
            new ChatMessage(ChatRole.System, [new TextContent(prompts.SurvivalCapabilityReasonPrompt)]),
            new ChatMessage(ChatRole.User, [new TextContent(gameDescription)]),
        };

        var chatResponse = await _chatClient
            .GetStreamingResponseAsync(messages, _chatOptions, cancellationToken)
            .ToChatResponseAsync(cancellationToken);

        try
        {
            var chatResult = chatResponse.ToString();
            var jsonMatch = JsonRegex().Match(chatResult);

            return JsonSerializer.Deserialize<BunkerSurvivalCapabilityResult>(
                jsonMatch.ValueSpan,
                _jsonSerializerOptions
            )!;
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Failed deserialize chat result. Return negative survival capability. Chat response: {ChatResponse}",
                chatResponse.ToString()
            );

            return new BunkerSurvivalCapabilityResult(false);
        }
    }

    [GeneratedRegex(@"\{[\s\S]*?\}")]
    private static partial Regex JsonRegex();
}
