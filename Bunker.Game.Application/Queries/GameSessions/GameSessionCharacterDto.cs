namespace Bunker.Game.Application.Queries.GameSessions
{
    public record GameSessionCharacterDto
    {
        public Guid Id { get; set; }
        public string? PlayerId { get; set; }
        public string? PlayerName { get; set; }
        public bool IsOccupiedByPlayer { get; set; }
        public bool IsGameCreator { get; set; }
        public bool IsKicked { get; set; }
    }
}
