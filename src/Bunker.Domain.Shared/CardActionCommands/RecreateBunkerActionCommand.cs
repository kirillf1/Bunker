using Bunker.Game.Domain.AggregateModels.Characters.Cards.CardActions;

namespace Bunker.Domain.Shared.CardActionCommands;

public class RecreateBunkerActionCommand : CardActionCommand
{
    public Guid GameSessionId { get; }

    public RecreateBunkerActionCommand(Guid gameSessionId)
    {
        GameSessionId = gameSessionId;
    }
}
