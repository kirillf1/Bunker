using Bunker.GameComponents.API.Models.CharacterComponents.Cards;

namespace Bunker.GameComponents.API.Models;

public class DefaultGameComponentsModel
{
    public List<string> Catastrophes { get; set; } = new();
    public List<string> BunkerDescriptions { get; set; } = new();
    public List<string> BunkerRooms { get; set; } = new();
    public List<string> BunkerEnvironments { get; set; } = new();
    public List<string> BunkerItems { get; set; } = new();
    public List<string> Professions { get; set; } = new();
    public List<string> Phobias { get; set; } = new();
    public List<string> AdditionalInformations { get; set; } = new();
    public List<string> Health { get; set; } = new();
    public List<string> Traits { get; set; } = new();
    public List<string> Hobbies { get; set; } = new();
    public List<string> Items { get; set; } = new();
    public List<CardCreateDto> Cards { get; set; } = new();
} 