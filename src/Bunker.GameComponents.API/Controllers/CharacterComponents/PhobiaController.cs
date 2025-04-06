using Bunker.GameComponents.API.Entities.CharacterComponents;
using Bunker.GameComponents.API.Infrastructure.Database;
using Bunker.GameComponents.API.Models.CharacterComponents.Phobia;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Bunker.GameComponents.API.Controllers.CharacterComponents;

[Route("api/character-components/phobias")]
[ApiController]
[Produces("application/json")]
public class PhobiaController : ControllerBase
{
    private readonly GameComponentsContext _context;

    public PhobiaController(GameComponentsContext context)
    {
        _context = context;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<PhobiaDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<IEnumerable<PhobiaDto>>> GetAll()
    {
        var phobias = await _context
            .Phobias.Select(p => new PhobiaDto { Id = p.Id, Description = p.Description })
            .ToListAsync();

        return Ok(phobias);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(PhobiaDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<PhobiaDto>> GetById(Guid id)
    {
        var phobia = await _context.Phobias.FirstOrDefaultAsync(p => p.Id == id);
        if (phobia is null)
        {
            return NotFound();
        }

        var dto = new PhobiaDto { Id = phobia.Id, Description = phobia.Description };
        return Ok(dto);
    }

    [HttpPost]
    [ProducesResponseType(typeof(PhobiaDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<PhobiaDto>> Create([FromBody] CreatePhobiaDto dto)
    {
        var phobia = new PhobiaEntity(dto.Description);
        _context.Phobias.Add(phobia);
        await _context.SaveChangesAsync();

        var resultDto = new PhobiaDto { Id = phobia.Id, Description = phobia.Description };
        return CreatedAtAction(nameof(GetById), new { id = phobia.Id }, resultDto);
    }

    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdatePhobiaDto dto)
    {
        var phobia = await _context.Phobias.FirstOrDefaultAsync(p => p.Id == id);
        if (phobia is null)
        {
            return NotFound();
        }

        phobia.Description = dto.Description;
        _context.Update(phobia);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Delete(Guid id)
    {
        var phobia = await _context.Phobias.FirstOrDefaultAsync(p => p.Id == id);
        if (phobia is null)
        {
            return NotFound();
        }

        _context.Phobias.Remove(phobia);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
