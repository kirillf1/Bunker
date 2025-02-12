namespace Bunker.Game.Domain.AggregateModels.Characters.Cards.CardActions;

public class SpyBunkerGameComponent : CardAction
{
    public BunkerObjectType BunkerObjectType { get; }

    public SpyBunkerGameComponent(CardActionRequirements cardActionRequirements, BunkerObjectType bunkerObjectType)
        : base(cardActionRequirements)
    {
        BunkerObjectType = bunkerObjectType;
    }

    public override CardActionCommand CreateActionCommand(ActivateCardParams activateCardParams)
    {
        return new SpyBunkerEnvironmentActionCommand(BunkerObjectType);
    }

    public override CardActionRequirements GetCurrentCardActionRequirements()
    {
        return new CardActionRequirements(ActivateCardTargetType.None, 0);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return BunkerObjectType;
    }

    public class SpyBunkerEnvironmentActionCommand : CardActionCommand
    {
        public BunkerObjectType BunkerObjectType { get; }

        public SpyBunkerEnvironmentActionCommand(BunkerObjectType bunkerObjectType)
        {
            BunkerObjectType = bunkerObjectType;
        }
    }
}
