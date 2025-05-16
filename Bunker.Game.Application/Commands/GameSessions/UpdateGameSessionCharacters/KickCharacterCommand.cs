namespace Bunker.Game.Application.Commands.GameSessions.UpdateGameSessionCharacters;

public record KickCharacterCommand(Guid GameSessionId, Guid CharacterId);
