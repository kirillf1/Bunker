namespace Bunker.Game.Application.Queries.Characters.Models;

public record CharacterDto
{
    public Guid Id { get; set; }
    public Guid GameSessionId { get; set; }
    public bool IsKicked { get; set; }

    public string AdditionalInformation { get; set; } = string.Empty;
    public int Age { get; set; }
    public bool CanGiveBirth { get; set; }
    public string Health { get; set; } = string.Empty;
    public HobbyDto Hobby { get; set; } = null!;
    public string Phobia { get; set; } = string.Empty;
    public ProfessionDto Profession { get; set; } = null!;
    public string Sex { get; set; } = string.Empty;
    public SizeDto Size { get; set; } = null!;

    public List<CharacterItemDto> Items { get; set; } = new();
    public List<TraitDto> Traits { get; set; } = new();
    public List<CardDto> Cards { get; set; } = new();
}
