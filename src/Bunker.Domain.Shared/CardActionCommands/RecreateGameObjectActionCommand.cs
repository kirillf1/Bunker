using Bunker.Domain.Shared.GameComponents;
using Bunker.Game.Domain.AggregateModels.Characters.Cards.CardActions;

namespace Bunker.Domain.Shared.CardActionCommands;

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
