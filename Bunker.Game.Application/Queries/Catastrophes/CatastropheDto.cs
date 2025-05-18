namespace Bunker.Game.Application.Queries.Catastrophes;

public record CatastropheDto
{
    public Guid Id { get; set; }
    public string Description { get; set; } = string.Empty;
    public bool IsReadonly { get; set; }
}
