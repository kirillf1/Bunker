namespace Bunker.ResultCreator.API.Domain.GameResultPrompts;

public class SurvivalScenarioPrompt
{
    public string ReproductionCapabilityReasonPrompt { get; set; }

    public string SurvivalCapabilityReasonPrompt { get; set; }

    public string PositiveBunkerLifeHistoryPrompt { get; set; }
    public string NegativeBunkerLifeHistoryPrompt { get; set; }

    public SurvivalScenarioPrompt(
        string reproductionCapabilityReasonPrompt,
        string survivalCapabilityReasonPrompt,
        string bunkerLifeHistoryPrompt,
        string negativeBunkerLifeHistoryPrompt
    )
    {
        ReproductionCapabilityReasonPrompt = reproductionCapabilityReasonPrompt;
        SurvivalCapabilityReasonPrompt = survivalCapabilityReasonPrompt;
        PositiveBunkerLifeHistoryPrompt = bunkerLifeHistoryPrompt;
        NegativeBunkerLifeHistoryPrompt = negativeBunkerLifeHistoryPrompt;
    }
}
