namespace Bunker.Game.Application.Queries.Bunkers;

public record ItemDto
{
    public string Description { get; set; } = string.Empty;
    public bool IsHidden { get; set; }
}
