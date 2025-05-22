namespace Bunker.Game.Application.Queries.Bunkers;

public record RoomDto
{
    public string Description { get; set; } = string.Empty;
    public bool IsHidden { get; set; }
}
