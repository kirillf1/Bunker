namespace Bunker.Game.Application.Queries.Characters.Models;

public record CardDto
{
    public Guid Id { get; set; }
    public string Description { get; set; } = string.Empty;
    public bool IsActivated { get; set; }
}
