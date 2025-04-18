using Bunker.Domain.Shared.Cards.CardActionCommands;

namespace Bunker.Game.Domain.AggregateModels.Characters.Cards.CardActions;

/// <summary>
// For cards that cannot be implemented through code.
// That is, what the players must do, for example, not say anything next circle
/// </summary>
public partial class EmptyAction : CardAction
{
    public EmptyAction() { }

    public override CardActionCommand CreateActionCommand(ActivateCardParams activateCardParams, Guid gameSessionId)
    {
        return new EmptyActionCommand();
    }

    public override CardActionRequirements GetCurrentCardActionRequirements()
    {
        return new CardActionRequirements(ActivateCardTargetType.None, 0);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Array.Empty<object>();
    }
}
