namespace Bunker.Domain.Shared.Cards.CardActionCommands;

public class RecreateCatastropheActionCommand : CardActionCommand
{
    public Guid GameSessionId { get; }

    public RecreateCatastropheActionCommand(Guid gameSessionId)
    {
        GameSessionId = gameSessionId;
    }
}
