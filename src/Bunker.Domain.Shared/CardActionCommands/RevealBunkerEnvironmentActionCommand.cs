using Bunker.Domain.Shared.GameComponents;
using Bunker.Game.Domain.AggregateModels.Characters.Cards.CardActions;

namespace Bunker.Domain.Shared.CardActionCommands;

public class RevealBunkerEnvironmentActionCommand : CardActionCommand
{
    public BunkerObjectType BunkerObjectType { get; }

    public RevealBunkerEnvironmentActionCommand(BunkerObjectType bunkerObjectType)
    {
        BunkerObjectType = bunkerObjectType;
    }
}
