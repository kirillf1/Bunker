using Bunker.GameComponents.API.Entities.BunkerComponents;
using Bunker.GameComponents.API.Infrastructure.Database;
using Bunker.GameComponents.API.Models.BunkerComponents.Descriptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Bunker.GameComponents.API.Controllers.BunkerComponents;

[Route("api/bunker-components/descriptions")]
[ApiController]
[Produces("application/json")]
public class BunkerDescriptionsController : ControllerBase
{
    private readonly GameComponentsContext _context;

    public BunkerDescriptionsController(GameComponentsContext context)
    {
        _context = context;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<BunkerDescriptionDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<IEnumerable<BunkerDescriptionDto>>> GetBunkerDescriptions()
    {
        var descriptions = await _context
            .BunkerDescriptions.Select(b => new BunkerDescriptionDto { Id = b.Id, Text = b.Text })
            .ToListAsync();

        return Ok(descriptions);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(BunkerDescriptionDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<BunkerDescriptionDto>> GetBunkerDescription(Guid id)
    {
        var description = await _context.BunkerDescriptions.FirstOrDefaultAsync(x => x.Id == id);
        if (description is null)
        {
            return NotFound();
        }

        var dto = new BunkerDescriptionDto { Id = description.Id, Text = description.Text };
        return Ok(dto);
    }

    [HttpPost]
    [ProducesResponseType(typeof(BunkerDescriptionDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<BunkerDescriptionDto>> CreateBunkerDescription([FromBody] CreateBunkerDescriptionDto dto)
    {
        var description = new BunkerDescriptionEntity(dto.Text);
        _context.BunkerDescriptions.Add(description);
        await _context.SaveChangesAsync();

        var resultDto = new BunkerDescriptionDto { Id = description.Id, Text = description.Text };
        return CreatedAtAction(nameof(GetBunkerDescription), new { id = description.Id }, resultDto);
    }

    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateBunkerDescription(Guid id, [FromBody] UpdateBunkerDescriptionDto dto)
    {
        var description = await _context.BunkerDescriptions.FirstOrDefaultAsync(x => x.Id == id);
        if (description is null)
        {
            return NotFound();
        }

        description.Text = dto.Text;
        _context.Update(description);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteBunkerDescription(Guid id)
    {
        var description = await _context.BunkerDescriptions.FirstOrDefaultAsync(x => x.Id == id);
        if (description is null)
        {
            return NotFound();
        }

        _context.BunkerDescriptions.Remove(description);
        await _context.SaveChangesAsync();

        return NoContent();
    }
} 