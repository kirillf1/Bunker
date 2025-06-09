using System.Text.Json;
using Bunker.ResultCreator.API.Domain.GameResultPrompts;
using Microsoft.Extensions.Options;

namespace Bunker.ResultCreator.API.Infrastructure.PromptStorage;

public class JsonPromptStorage : IPromptStorage
{
    private readonly string _promptsFilePath;
    private readonly ILogger<JsonPromptStorage> _logger;
    private SurvivalScenarioPrompt? _cachedPrompts;
    private DateTime? _lastFileWriteTime;
    private readonly object _lockObject = new object();

    public JsonPromptStorage(IOptions<PromptStorageOptions> options, ILogger<JsonPromptStorage> logger)
    {
        _logger = logger;
        _promptsFilePath = GetPromptsFilePath(options.Value);
    }

    public async Task<SurvivalScenarioPrompt> GetSurvivalScenarioPrompt()
    {
        lock (_lockObject)
        {
            if (ShouldReloadFile())
            {
                _cachedPrompts = null;
                _lastFileWriteTime = null;
                _logger.LogInformation("File changed detected. Cache invalidated.");
            }

            if (_cachedPrompts is not null)
            {
                return _cachedPrompts;
            }
        }

        return await LoadPromptsFromFile();
    }

    private bool ShouldReloadFile()
    {
        if (!File.Exists(_promptsFilePath))
        {
            return true;
        }

        var currentFileWriteTime = File.GetLastWriteTime(_promptsFilePath);

        if (!_lastFileWriteTime.HasValue)
        {
            return true;
        }

        return currentFileWriteTime > _lastFileWriteTime.Value;
    }

    private string GetPromptsFilePath(PromptStorageOptions options)
    {
        if (string.IsNullOrWhiteSpace(options.FilePath))
        {
            var defaultPath = Path.Combine(
                AppContext.BaseDirectory,
                "Infrastructure",
                "PromptStorage",
                "default_prompts.json"
            );
            _logger.LogInformation("Config path is empty, using default path: {DefaultPath}", defaultPath);
            return defaultPath;
        }

        var configuredPath = options.FilePath;
        if (File.Exists(configuredPath))
        {
            _logger.LogInformation("Using configured path: {ConfiguredPath}", configuredPath);
            return configuredPath;
        }

        var fallbackPath = Path.Combine(
            AppContext.BaseDirectory,
            "Infrastructure",
            "PromptStorage",
            "default_prompts.json"
        );
        _logger.LogWarning(
            "Configured file not found at {ConfiguredPath}, falling back to default: {FallbackPath}",
            configuredPath,
            fallbackPath
        );
        return fallbackPath;
    }

    private async Task<SurvivalScenarioPrompt> LoadPromptsFromFile()
    {
        if (!File.Exists(_promptsFilePath))
        {
            var errorMessage =
                $"Prompts file not found at {_promptsFilePath}. Please ensure the file exists and is accessible.";
            _logger.LogError(errorMessage);
            throw new FileNotFoundException(errorMessage, _promptsFilePath);
        }

        try
        {
            var fileWriteTime = File.GetLastWriteTime(_promptsFilePath);
            var jsonContent = await File.ReadAllTextAsync(_promptsFilePath);
            var promptsData = JsonSerializer.Deserialize<PromptsData>(
                jsonContent,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
            );

            if (promptsData?.SurvivalScenario is null)
            {
                var errorMessage =
                    $"Invalid prompts file format at {_promptsFilePath}. Expected 'SurvivalScenario' section not found.";
                _logger.LogError(errorMessage);
                throw new InvalidDataException(errorMessage);
            }

            var loadedPrompts = new SurvivalScenarioPrompt(
                promptsData.SurvivalScenario.ReproductionCapabilityReasonPrompt,
                promptsData.SurvivalScenario.SurvivalCapabilityReasonPrompt,
                promptsData.SurvivalScenario.PositiveBunkerLifeHistoryPrompt,
                promptsData.SurvivalScenario.NegativeBunkerLifeHistoryPrompt
            );

            lock (_lockObject)
            {
                _cachedPrompts = loadedPrompts;
                _lastFileWriteTime = fileWriteTime;
            }

            _logger.LogInformation(
                "Successfully loaded prompts from file: {FilePath} (modified: {ModifiedTime})",
                _promptsFilePath,
                fileWriteTime
            );

            return loadedPrompts;
        }
        catch (JsonException ex)
        {
            var errorMessage = $"Invalid JSON format in prompts file at {_promptsFilePath}.";
            _logger.LogError(ex, errorMessage);
            throw new InvalidDataException(errorMessage, ex);
        }
        catch (Exception ex) when (ex is not FileNotFoundException && ex is not InvalidDataException)
        {
            var errorMessage = $"Error loading prompts from file {_promptsFilePath}.";
            _logger.LogError(ex, errorMessage);
            throw new InvalidOperationException(errorMessage, ex);
        }
    }

    public class PromptsData
    {
        public SurvivalScenarioPromptData? SurvivalScenario { get; set; }
    }

    public class SurvivalScenarioPromptData
    {
        public string ReproductionCapabilityReasonPrompt { get; set; } = string.Empty;
        public string SurvivalCapabilityReasonPrompt { get; set; } = string.Empty;
        public string PositiveBunkerLifeHistoryPrompt { get; set; } = string.Empty;
        public string NegativeBunkerLifeHistoryPrompt { get; set; } = string.Empty;
    }
}
