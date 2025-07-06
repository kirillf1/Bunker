namespace Bunker.ResultCreator.API.Domain.SurvivalPredictor;

public class BunkerSurvivalCapabilityResult
{
    public bool CanSurvive { get; private set; }

    public string? Reason { get; private set; }

    public BunkerSurvivalCapabilityResult(bool canSurvive, string? reason = null)
    {
        CanSurvive = canSurvive;
        Reason = reason;
    }
}
