namespace Bunker.Game.API.Application.Commands.GameSessions.UpdateGameSessionCharacters;

public record KickCharacterCommand(Guid GameSessionId, Guid CharacterId);
