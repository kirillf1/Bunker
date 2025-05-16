namespace Bunker.Game.Application.Commands.GameSessions.UpdateGameSessionState;

public record EndGameSessionCommand(Guid GameSessionId, string GameResultDescription);
