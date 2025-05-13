namespace Bunker.Game.API.Application.Commands.GameSessions.UpdateGameSessionState;

public record EndGameSessionCommand(Guid GameSessionId, string GameResultDescription);
