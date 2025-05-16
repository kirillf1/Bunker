namespace Bunker.Game.Application.Queries.Characters.Models;

public record ItemDto
{
    public Guid Id { get; set; }
    public string Description { get; set; } = string.Empty;
}
