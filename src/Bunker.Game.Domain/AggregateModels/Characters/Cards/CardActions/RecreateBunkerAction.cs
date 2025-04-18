using Bunker.Domain.Shared.Cards.CardActionCommands;

namespace Bunker.Game.Domain.AggregateModels.Characters.Cards.CardActions;

public class RecreateBunkerAction : CardAction
{
    public RecreateBunkerAction() { }

    public override CardActionCommand CreateActionCommand(ActivateCardParams activateCardParams, Guid gameSessionId)
    {
        return new RecreateBunkerActionCommand(gameSessionId);
    }

    public override CardActionRequirements GetCurrentCardActionRequirements()
    {
        return new CardActionRequirements(ActivateCardTargetType.None, 0);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return this;
    }
}
