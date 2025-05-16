namespace Bunker.Game.Application.Queries.Characters.Models;

public record HobbyDto
{
    public string Description { get; set; } = string.Empty;
    public byte Experience { get; set; }
}
