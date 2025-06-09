namespace Bunker.ResultCreator.API.Domain.SurvivalPredictor;

public class BunkerReproductionCapabilityResult
{
    public bool CanGiveBirth { get; private set; }

    public string? Reason { get; private set; }

    public BunkerReproductionCapabilityResult(bool canGiveBirth, string? reason = null)
    {
        CanGiveBirth = canGiveBirth;
        Reason = reason;
    }
}
