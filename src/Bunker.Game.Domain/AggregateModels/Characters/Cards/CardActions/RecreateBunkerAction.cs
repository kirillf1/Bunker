using Bunker.Domain.Shared.CardActionCommands;

namespace Bunker.Game.Domain.AggregateModels.Characters.Cards.CardActions;

public class RecreateBunkerAction : CardAction
{
    public RecreateBunkerAction(CardActionRequirements cardActionRequirements)
        : base(cardActionRequirements) { }

    public override CardActionCommand CreateActionCommand(ActivateCardParams activateCardParams)
    {
        return new RecreateBunkerActionCommand();
    }

    public override CardActionRequirements GetCurrentCardActionRequirements()
    {
        return new CardActionRequirements(ActivateCardTargetType.None, 0);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return CardActionRequirements;
    }
}
