using Bunker.GameComponents.API.Entities.CharacterComponents;
using Bunker.GameComponents.API.Infrastructure.Database;
using Bunker.GameComponents.API.Models.CharacterComponents.Item;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Bunker.GameComponents.API.Controllers.CharacterComponents;

[Route("api/character-components/items")]
[ApiController]
[Produces("application/json")]
public class ItemController : ControllerBase
{
    private readonly GameComponentsContext _context;

    public ItemController(GameComponentsContext context)
    {
        _context = context;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<ItemDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<IEnumerable<ItemDto>>> GetAll()
    {
        var items = await _context
            .Items.Select(p => new ItemDto { Id = p.Id, Description = p.Description })
            .ToListAsync();

        return Ok(items);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ItemDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ItemDto>> GetById(Guid id)
    {
        var item = await _context.Items.FirstOrDefaultAsync(p => p.Id == id);
        if (item is null)
        {
            return NotFound();
        }

        var dto = new ItemDto { Id = item.Id, Description = item.Description };
        return Ok(dto);
    }

    [HttpPost]
    [ProducesResponseType(typeof(ItemDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<ItemDto>> Create([FromBody] CreateItemDto dto)
    {
        var item = new ItemEntity(dto.Description);
        _context.Items.Add(item);
        await _context.SaveChangesAsync();

        var resultDto = new ItemDto { Id = item.Id, Description = item.Description };
        return CreatedAtAction(nameof(GetById), new { id = item.Id }, resultDto);
    }

    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateItemDto dto)
    {
        var item = await _context.Items.FirstOrDefaultAsync(p => p.Id == id);
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
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id)
    {
        var item = await _context.Items.FirstOrDefaultAsync(p => p.Id == id);
        if (item is null)
        {
            return NotFound();
        }

        _context.Items.Remove(item);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
