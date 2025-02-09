namespace Bunker.Game.Domain.AggregateModels.Characters.Cards.CardActions;

/// <summary>
// For cards that cannot be implemented through code.
// That is, what the players must do, for example, not say anything next circle
/// </summary>
public class EmptyAction : CardAction
{
    public EmptyAction(CardActionRequirements cardActionRequirements)
        : base(cardActionRequirements) { }

    public override CardActionCommand CreateActionCommand(ActivateCardParams activateCardParams)
    {
        return new EmptyActionCommand();
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Array.Empty<object>();
    }

    public class EmptyActionCommand : CardActionCommand;
}
