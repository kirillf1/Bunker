using Bunker.GameComponents.API.Entities.BunkerComponents;
using Bunker.GameComponents.API.Infrastructure.Database;
using Bunker.GameComponents.API.Models.BunkerComponents.Items;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Bunker.GameComponents.API.Controllers.BunkerComponents;

[Route("api/bunker-components/items")]
[ApiController]
[Produces("application/json")]
public class BunkerItemsController : ControllerBase
{
    private readonly GameComponentsContext _context;

    public BunkerItemsController(GameComponentsContext context)
    {
        _context = context;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<BunkerItemDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<IEnumerable<BunkerItemDto>>> GetBunkerItems()
    {
        var items = await _context
            .BunkerItems.Select(b => new BunkerItemDto { Id = b.Id, Description = b.Description })
            .ToListAsync();

        return Ok(items);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(BunkerItemDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<BunkerItemDto>> GetBunkerItem(Guid id)
    {
        var item = await _context.BunkerItems.FirstOrDefaultAsync(x => x.Id == id);
        if (item is null)
        {
            return NotFound();
        }

        var dto = new BunkerItemDto { Id = item.Id, Description = item.Description };
        return Ok(dto);
    }

    [HttpPost]
    [ProducesResponseType(typeof(BunkerItemDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<BunkerItemDto>> CreateBunkerItem([FromBody] CreateBunkerItemDto dto)
    {
        var item = new BunkerItemEntity(dto.Description);
        _context.BunkerItems.Add(item);
        await _context.SaveChangesAsync();

        var resultDto = new BunkerItemDto { Id = item.Id, Description = item.Description };

        return CreatedAtAction(nameof(GetBunkerItem), new { id = item.Id }, resultDto);
    }

    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateBunkerItem(Guid id, [FromBody] UpdateBunkerItemDto dto)
    {
        var item = await _context.BunkerItems.FirstOrDefaultAsync(x => x.Id == id);
        if (item is null)
        {
            return NotFound();
        }

        item.Description = dto.Description;

        _context.Update(item);

        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteBunkerItem(Guid id)
    {
        var item = await _context.BunkerItems.FirstOrDefaultAsync(x => x.Id == id);
        if (item is null)
        {
            return NotFound();
        }

        _context.BunkerItems.Remove(item);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
