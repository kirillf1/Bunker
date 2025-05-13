namespace Bunker.Game.API.Application.Commands.GameSessions.CreateGameSession;

public record CreateGameSessionCommand(
    Guid GameSessionId,
    string Name,
    string PlayerId,
    string PlayerName,
    int CharactersCount
);
