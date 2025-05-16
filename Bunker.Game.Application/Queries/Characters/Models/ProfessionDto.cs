namespace Bunker.Game.Application.Queries.Characters.Models;

public record ProfessionDto
{
    public string Description { get; set; } = string.Empty;
    public byte Experience { get; set; }
}
