namespace Bunker.Game.API.Application.Commands.GameSessions.UpdateGameSessionCharacters;

public class OccupyCharacterCommand
{
    public Guid GameSessionId { get; }
    public string PlayerId { get; }
    public string PlayerName { get; }

    public OccupyCharacterCommand(Guid gameSessionId, string playerId, string playerName)
    {
        GameSessionId = gameSessionId;
        PlayerId = playerId;
        PlayerName = playerName;
    }
}
