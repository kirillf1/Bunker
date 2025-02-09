namespace Bunker.Game.Domain.AggregateModels.Characters.Cards.CardActions;

public class SpyBunkerEnvironment : CardAction
{
    public BunkerObjectType BunkerObjectType { get; }

    public SpyBunkerEnvironment(CardActionRequirements cardActionRequirements, BunkerObjectType bunkerObjectType)
        : base(cardActionRequirements)
    {
        BunkerObjectType = bunkerObjectType;
    }

    public override CardActionCommand CreateActionCommand(ActivateCardParams activateCardParams)
    {
        return new SpyBunkerEnvironmentActionCommand(BunkerObjectType);
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

public class RecreateGameObject : CardAction
{
    public GameObjectType GameObjectType { get; }

    public RecreateGameObject(CardActionRequirements cardActionRequirements, GameObjectType gameObjectType)
        : base(cardActionRequirements)
    {
        GameObjectType = gameObjectType;
    }

    public override CardActionCommand CreateActionCommand(ActivateCardParams activateCardParams)
    {
        return new RecreateGameObjectActionCommand(GameObjectType);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return GameObjectType;
    }

    public class RecreateGameObjectActionCommand : CardActionCommand
    {
        public GameObjectType GameObjectType { get; }

        public RecreateGameObjectActionCommand(GameObjectType gameObjectType)
        {
            GameObjectType = gameObjectType;
        }
    }
}
