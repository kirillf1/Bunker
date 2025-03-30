using Bunker.Domain.Shared.GameComponents;

namespace Bunker.Domain.Shared.Cards.CardActionCommands;

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
