using Bunker.Domain.Shared.CardActionCommands;

namespace Bunker.Game.Domain.AggregateModels.Characters.Cards.CardActions;

public class RecreateCatastropheAction : CardAction
{
    public RecreateCatastropheAction(CardActionRequirements cardActionRequirements)
        : base(cardActionRequirements) { }

    public override CardActionCommand CreateActionCommand(ActivateCardParams activateCardParams)
    {
        return new RecreateCatastropheActionCommand();
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
