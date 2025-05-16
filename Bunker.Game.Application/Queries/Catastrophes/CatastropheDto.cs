namespace Bunker.Game.Application.Queries.Catastrophes;

public record CatastropheDto
{
    public Guid Id { get; set; }
    public Guid GameSessionId { get; set; }
    public string Description { get; set; } = string.Empty;
}
