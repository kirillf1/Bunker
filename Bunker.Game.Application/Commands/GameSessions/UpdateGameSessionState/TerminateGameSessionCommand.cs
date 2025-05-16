namespace Bunker.Game.Application.Commands.GameSessions.UpdateGameSessionState;

public class TerminateGameSessionCommand
{
    public Guid GameSessionId { get; }

    public TerminateGameSessionCommand(Guid gameSessionId)
    {
        GameSessionId = gameSessionId;
    }
}
