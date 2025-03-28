using Bunker.Game.Domain.AggregateModels.Characters.Cards.CardActions;

namespace Bunker.Domain.Shared.CardActionCommands;

public class RecreateCatastropheActionCommand : CardActionCommand
{
    public Guid GameSessionId { get; }

    public RecreateCatastropheActionCommand(Guid gameSessionId)
    {
        GameSessionId = gameSessionId;
    }
}
