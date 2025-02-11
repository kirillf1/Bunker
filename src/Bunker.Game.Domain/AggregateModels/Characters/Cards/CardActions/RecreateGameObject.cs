namespace Bunker.Game.Domain.AggregateModels.Characters.Cards.CardActions;

/// <summary>
/// If you need more fine-tuning (for example, 2 players at once) of the map, you can split the class.
/// For example on RecreateCharacter, etc.
/// </summary>
public class RecreateGameObject : CardAction
{
    public GameObjectType GameObjectType { get; }

    public RecreateGameObject(CardActionRequirements cardActionRequirements, GameObjectType gameObjectType)
        : base(cardActionRequirements)
    {
        GameObjectType = gameObjectType;
    }

    public override CardActionRequirements GetCurrentCardActionRequirements()
    {
        var targetType =
            GameObjectType == GameObjectType.Character ? ActivateCardTargetType.Character : ActivateCardTargetType.None;

        var charactersCount = targetType == ActivateCardTargetType.Character ? 1 : 0;

        return new CardActionRequirements(targetType, charactersCount);
    }

    public override CardActionCommand CreateActionCommand(ActivateCardParams activateCardParams)
    {
        Guid? targetId = null;

        if (GameObjectType == GameObjectType.Character)
        {
            targetId =
                activateCardParams.TargetCharacterIds.Count() == 1
                    ? activateCardParams.TargetCharacterIds.First()
                    : throw new ArgumentException("To recreate character need one target character id");
        }

        return new RecreateGameObjectActionCommand(GameObjectType, targetId);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return GameObjectType;
    }

    public class RecreateGameObjectActionCommand : CardActionCommand
    {
        public GameObjectType GameObjectType { get; }

        public Guid? TargetId { get; }

        public RecreateGameObjectActionCommand(GameObjectType gameObjectType, Guid? targetId = null)
        {
            GameObjectType = gameObjectType;
            TargetId = targetId;
        }
    }
}
