using Bunker.Domain.Shared.Cards.CardActionCommands;

namespace Bunker.Game.Domain.AggregateModels.Characters.Cards.CardActions;

public class RevealBunkerGameComponentCardAction : CardAction
{
    public BunkerObjectType BunkerObjectType { get; }

    public RevealBunkerGameComponentCardAction(BunkerObjectType bunkerObjectType)
    {
        BunkerObjectType = bunkerObjectType;
    }

    public override CardActionCommand CreateActionCommand(ActivateCardParams activateCardParams, Guid gameSessionId)
    {
        return new RevealBunkerComponentActionCommand(BunkerObjectType, gameSessionId);
    }

    public override CardActionRequirements GetCurrentCardActionRequirements()
    {
        return new CardActionRequirements(ActivateCardTargetType.None, 0);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return BunkerObjectType;
    }
}
