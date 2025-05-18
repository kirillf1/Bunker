using Microsoft.AspNetCore.Mvc;

namespace Bunker.Game.API.Models.Characters;

public class UseCardRequest
{
    public IEnumerable<Guid>? TargetCharactersId { get; set; }
}
