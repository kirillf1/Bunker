using Bunker.GameComponents.API.Entities.CatastropheComponents;
using Bunker.GameComponents.API.Infrastructure;
using Bunker.GameComponents.API.Models.Catastrophes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Bunker.GameComponents.API.Controllers;

[Route("api/catastrophes")]
[ApiController]
[Produces("application/json")]
public class CatastrophesController : ControllerBase
{
    private readonly GameComponentsContext _context;

    public CatastrophesController(GameComponentsContext context)
    {
        _context = context;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<CatastropheDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<IEnumerable<CatastropheDto>>> GetAll()
    {
        var catastrophes = await _context
            .Catastrophes.Select(p => new CatastropheDto { Id = p.Id, Description = p.Description })
            .ToListAsync();

        return Ok(catastrophes);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(CatastropheDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<CatastropheDto>> GetById(Guid id)
    {
        var catastrophe = await _context.Catastrophes.FirstOrDefaultAsync(p => p.Id == id);
        if (catastrophe is null)
        {
            return NotFound();
        }

        var dto = new CatastropheDto { Id = catastrophe.Id, Description = catastrophe.Description };
        return Ok(dto);
    }

    [HttpPost]
    [ProducesResponseType(typeof(CatastropheDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<CatastropheDto>> Create([FromBody] CreateCatastropheDto dto)
    {
        var catastrophe = new CatastropheEntity(dto.Description);
        _context.Catastrophes.Add(catastrophe);
        await _context.SaveChangesAsync();

        var resultDto = new CatastropheDto { Id = catastrophe.Id, Description = catastrophe.Description };
        return CreatedAtAction(nameof(GetById), new { id = catastrophe.Id }, resultDto);
    }

    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateCatastropheDto dto)
    {
        var catastrophe = await _context.Catastrophes.FirstOrDefaultAsync(p => p.Id == id);
        if (catastrophe is null)
        {
            return NotFound();
        }

        catastrophe.Description = dto.Description;
        _context.Update(catastrophe);
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
        var catastrophe = await _context.Catastrophes.FirstOrDefaultAsync(p => p.Id == id);
        if (catastrophe is null)
        {
            return NotFound();
        }

        _context.Catastrophes.Remove(catastrophe);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
