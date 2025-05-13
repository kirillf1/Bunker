namespace Bunker.Game.API.Application.Commands.GameSessions.CreateGameSession;

public record GameSessionCreationResult(Guid Id, Guid OccupiedCharacterId);
