using Bunker.Domain.Shared.CardActionCommands;

namespace Bunker.Game.Domain.AggregateModels.Characters.Cards.CardActions;

public class RevealBunkerGameComponentCardAction : CardAction
{
    public BunkerObjectType BunkerObjectType { get; }

    public RevealBunkerGameComponentCardAction(
        CardActionRequirements cardActionRequirements,
        BunkerObjectType bunkerObjectType
    )
        : base(cardActionRequirements)
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
        yield return CardActionRequirements;
        yield return BunkerObjectType;
    }
}
