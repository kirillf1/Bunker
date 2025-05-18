namespace Bunker.Game.API.Models.GameSessions;

public class OccupyCharacterRequest
{
    public required string PlayerId { get; set; }
    public required string PlayerName { get; set; }
} 