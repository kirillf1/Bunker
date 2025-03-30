namespace Bunker.Domain.Shared.Cards.CardActionCommands;

public class RecreateBunkerActionCommand : CardActionCommand
{
    public Guid GameSessionId { get; }

    public RecreateBunkerActionCommand(Guid gameSessionId)
    {
        GameSessionId = gameSessionId;
    }
}
