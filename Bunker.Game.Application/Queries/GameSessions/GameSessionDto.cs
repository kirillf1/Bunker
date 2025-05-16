using Bunker.Game.Domain.AggregateModels.GameSessions;

namespace Bunker.Game.Application.Queries.GameSessions;

public record GameSessionDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public GameState GameState { get; set; }
    public int FreeSeatsCount { get; set; }
    public string? GameResultDescription { get; set; }
    public List<GameSessionCharacterDto> Characters { get; set; } = new();
}
