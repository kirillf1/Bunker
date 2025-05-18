namespace Bunker.Game.API.Models.GameSessions;

public class CreateGameSessionRequest
{
    public required string Name { get; set; }
    public required string PlayerId { get; set; }
    public required string PlayerName { get; set; }
    public required int CharactersCount { get; set; }
}
