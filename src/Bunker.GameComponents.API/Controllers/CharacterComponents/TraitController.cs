using Bunker.GameComponents.API.Entities.CharacterComponents;
using Bunker.GameComponents.API.Infrastructure.Database;
using Bunker.GameComponents.API.Models.CharacterComponents.Trait;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Bunker.GameComponents.API.Controllers.CharacterComponents;

[Route("api/character-components/traits")]
[ApiController]
[Produces("application/json")]
public class TraitController : ControllerBase
{
    private readonly GameComponentsContext _context;

    public TraitController(GameComponentsContext context)
    {
        _context = context;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<TraitDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<IEnumerable<TraitDto>>> GetAll()
    {
        var traits = await _context
            .Traits.Select(p => new TraitDto { Id = p.Id, Description = p.Description })
            .ToListAsync();

        return Ok(traits);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(TraitDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<TraitDto>> GetById(Guid id)
    {
        var trait = await _context.Traits.FirstOrDefaultAsync(p => p.Id == id);
        if (trait is null)
        {
            return NotFound();
        }

        var dto = new TraitDto { Id = trait.Id, Description = trait.Description };
        return Ok(dto);
    }

    [HttpPost]
    [ProducesResponseType(typeof(TraitDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<TraitDto>> Create([FromBody] CreateTraitDto dto)
    {
        var trait = new TraitEntity(dto.Description);
        _context.Traits.Add(trait);
        await _context.SaveChangesAsync();

        var resultDto = new TraitDto { Id = trait.Id, Description = trait.Description };
        return CreatedAtAction(nameof(GetById), new { id = trait.Id }, resultDto);
    }

    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateTraitDto dto)
    {
        var trait = await _context.Traits.FirstOrDefaultAsync(p => p.Id == id);
        if (trait is null)
        {
            return NotFound();
        }

        trait.Description = dto.Description;
        _context.Update(trait);
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
        var trait = await _context.Traits.FirstOrDefaultAsync(p => p.Id == id);
        if (trait is null)
        {
            return NotFound();
        }

        _context.Traits.Remove(trait);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
