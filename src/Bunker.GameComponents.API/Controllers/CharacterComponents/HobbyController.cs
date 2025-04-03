using Bunker.GameComponents.API.Entities.CharacterComponents;
using Bunker.GameComponents.API.Infrastructure;
using Bunker.GameComponents.API.Models.CharacterComponents.Hobby;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Bunker.GameComponents.API.Controllers.CharacterComponents;

[Route("api/character-components/hobbies")]
[ApiController]
[Produces("application/json")]
public class HobbyController : ControllerBase
{
    private readonly GameComponentsContext _context;

    public HobbyController(GameComponentsContext context)
    {
        _context = context;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<HobbyDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<IEnumerable<HobbyDto>>> GetAll()
    {
        var hobbies = await _context
            .Hobbies.Select(p => new HobbyDto { Id = p.Id, Description = p.Description })
            .ToListAsync();

        return Ok(hobbies);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(HobbyDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<HobbyDto>> GetById(Guid id)
    {
        var hobby = await _context.Hobbies.FirstOrDefaultAsync(p => p.Id == id);
        if (hobby is null)
        {
            return NotFound();
        }

        var dto = new HobbyDto { Id = hobby.Id, Description = hobby.Description };
        return Ok(dto);
    }

    [HttpPost]
    [ProducesResponseType(typeof(HobbyDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<HobbyDto>> Create([FromBody] CreateHobbyDto dto)
    {
        var hobby = new HobbyEntity(dto.Description);
        _context.Hobbies.Add(hobby);
        await _context.SaveChangesAsync();

        var resultDto = new HobbyDto { Id = hobby.Id, Description = hobby.Description };
        return CreatedAtAction(nameof(GetById), new { id = hobby.Id }, resultDto);
    }

    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateHobbyDto dto)
    {
        var hobby = await _context.Hobbies.FirstOrDefaultAsync(p => p.Id == id);
        if (hobby is null)
        {
            return NotFound();
        }

        hobby.Description = dto.Description;
        _context.Update(hobby);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id)
    {
        var hobby = await _context.Hobbies.FirstOrDefaultAsync(p => p.Id == id);
        if (hobby is null)
        {
            return NotFound();
        }

        _context.Hobbies.Remove(hobby);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
