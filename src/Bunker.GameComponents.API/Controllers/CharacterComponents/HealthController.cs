using Bunker.GameComponents.API.Entities.CharacterComponents;
using Bunker.GameComponents.API.Infrastructure;
using Bunker.GameComponents.API.Models.CharacterComponents.HealthModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Bunker.GameComponents.API.Controllers.CharacterComponents;

[Route("api/character-components/health")]
[ApiController]
[Produces("application/json")]
public class HealthController : ControllerBase
{
    private readonly GameComponentsContext _context;

    public HealthController(GameComponentsContext context)
    {
        _context = context;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<HealthDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<IEnumerable<HealthDto>>> GetAll()
    {
        var healths = await _context
            .HealthEntitles.Select(p => new HealthDto { Id = p.Id, Description = p.Description })
            .ToListAsync();

        return Ok(healths);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(HealthDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<HealthDto>> GetById(Guid id)
    {
        var health = await _context.HealthEntitles.FirstOrDefaultAsync(p => p.Id == id);
        if (health is null)
        {
            return NotFound();
        }

        var dto = new HealthDto { Id = health.Id, Description = health.Description };
        return Ok(dto);
    }

    [HttpPost]
    [ProducesResponseType(typeof(HealthDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<HealthDto>> Create([FromBody] HealthCreateDto dto)
    {
        var health = new HealthEntity(dto.Description);
        _context.HealthEntitles.Add(health);
        await _context.SaveChangesAsync();

        var resultDto = new HealthDto { Id = health.Id, Description = health.Description };
        return CreatedAtAction(nameof(GetById), new { id = health.Id }, resultDto);
    }

    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(Guid id, [FromBody] HealthUpdateDto dto)
    {
        var health = await _context.HealthEntitles.FirstOrDefaultAsync(p => p.Id == id);
        if (health is null)
        {
            return NotFound();
        }

        health.Description = dto.Description;
        _context.Update(health);
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
        var health = await _context.HealthEntitles.FirstOrDefaultAsync(p => p.Id == id);
        if (health is null)
        {
            return NotFound();
        }

        _context.HealthEntitles.Remove(health);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
