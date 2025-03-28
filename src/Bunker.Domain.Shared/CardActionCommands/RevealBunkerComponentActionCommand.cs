using Bunker.Domain.Shared.GameComponents;
using Bunker.Game.Domain.AggregateModels.Characters.Cards.CardActions;

namespace Bunker.Domain.Shared.CardActionCommands;

public class RevealBunkerComponentActionCommand : CardActionCommand
{
    public BunkerObjectType BunkerObjectType { get; }

    public Guid GameSessionId { get; }

    public RevealBunkerComponentActionCommand(BunkerObjectType bunkerObjectType, Guid gameSessionId)
    {
        BunkerObjectType = bunkerObjectType;
        GameSessionId = gameSessionId;
    }
}
