using Bunker.GameComponents.API.Entities.CharacterComponents;
using Bunker.GameComponents.API.Infrastructure.Database;
using Bunker.GameComponents.API.Models.CharacterComponents.Profession;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Bunker.GameComponents.API.Controllers.CharacterComponents;

[Route("api/character-components/professions")]
[ApiController]
[Produces("application/json")]
public class ProfessionController : ControllerBase
{
    private readonly GameComponentsContext _context;

    public ProfessionController(GameComponentsContext context)
    {
        _context = context;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<ProfessionDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<IEnumerable<ProfessionDto>>> GetAll()
    {
        var professions = await _context
            .Professions.Select(p => new ProfessionDto { Id = p.Id, Description = p.Description })
            .ToListAsync();

        return Ok(professions);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ProfessionDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ProfessionDto>> GetById(Guid id)
    {
        var profession = await _context.Professions.FirstOrDefaultAsync(p => p.Id == id);
        if (profession is null)
        {
            return NotFound();
        }

        var dto = new ProfessionDto { Id = profession.Id, Description = profession.Description };
        return Ok(dto);
    }

    [HttpPost]
    [ProducesResponseType(typeof(ProfessionDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<ProfessionDto>> Create([FromBody] CreateProfessionDto dto)
    {
        var profession = new ProfessionEntity(dto.Description);
        _context.Professions.Add(profession);
        await _context.SaveChangesAsync();

        var resultDto = new ProfessionDto { Id = profession.Id, Description = profession.Description };
        return CreatedAtAction(nameof(GetById), new { id = profession.Id }, resultDto);
    }

    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateProfessionDto dto)
    {
        var profession = await _context.Professions.FirstOrDefaultAsync(p => p.Id == id);
        if (profession is null)
        {
            return NotFound();
        }

        profession.Description = dto.Description;
        _context.Update(profession);
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
        var profession = await _context.Professions.FirstOrDefaultAsync(p => p.Id == id);
        if (profession is null)
        {
            return NotFound();
        }

        _context.Professions.Remove(profession);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
