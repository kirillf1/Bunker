namespace Bunker.ResultCreator.API.Domain.GameResultPrompts;

public interface IPromptStorage
{
    Task<SurvivalScenarioPrompt> GetSurvivalScenarioPrompt();
}
