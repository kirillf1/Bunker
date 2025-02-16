using Bunker.Domain.Shared.CardActionCommands;

namespace Bunker.Game.Domain.AggregateModels.Characters.Cards.CardActions;

public class RevealBunkerGameComponent : CardAction
{
    public BunkerObjectType BunkerObjectType { get; }

    public RevealBunkerGameComponent(CardActionRequirements cardActionRequirements, BunkerObjectType bunkerObjectType)
        : base(cardActionRequirements)
    {
        BunkerObjectType = bunkerObjectType;
    }

    public override CardActionCommand CreateActionCommand(ActivateCardParams activateCardParams)
    {
        return new RevealBunkerEnvironmentActionCommand(BunkerObjectType);
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
