namespace Bunker.Game.Application.Commands.GameSessions.CreateGameSession;

public record GameSessionCreationResult(Guid Id, Guid OccupiedCharacterId);
