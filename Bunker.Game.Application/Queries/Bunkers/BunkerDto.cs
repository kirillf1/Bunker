namespace Bunker.Game.Application.Queries.Bunkers;

public record BunkerDto
{
    public Guid Id { get; set; }
    public Guid GameSessionId { get; set; }
    public string Description { get; set; } = string.Empty;
    public List<RoomDto> Rooms { get; set; } = new();
    public List<ItemDto> Items { get; set; } = new();
    public List<EnvironmentDto> Environments { get; set; } = new();
}
