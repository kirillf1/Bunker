using Bunker.GameComponents.API.Entities.BunkerComponents;
using Bunker.GameComponents.API.Infrastructure;
using Bunker.GameComponents.API.Models.BunkerComponents.Items;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Bunker.GameComponents.API.Controllers.BunkerComponents
{
    [Route("api/bunker-components/items")]
    [ApiController]
    public class BunkerItemsController : ControllerBase
    {
        private readonly GameComponentsContext _context;

        public BunkerItemsController(GameComponentsContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<BunkerItemDto>>> GetBunkerItems()
        {
            var items = await _context
                .BunkerItems.Select(b => new BunkerItemDto { Id = b.Id, Description = b.Description })
                .ToListAsync();

            return Ok(items);
        }

        [HttpGet("{id}")]
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
        public async Task<ActionResult<BunkerItemDto>> CreateBunkerItem([FromBody] CreateBunkerItemDto dto)
        {
            var item = new BunkerItemEntity(dto.Description);
            _context.BunkerItems.Add(item);
            await _context.SaveChangesAsync();

            var resultDto = new BunkerItemDto { Id = item.Id, Description = item.Description };

            return CreatedAtAction(nameof(GetBunkerItem), new { id = item.Id }, resultDto);
        }

        [HttpPut("{id}")]
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

        // DELETE: api/bunker-components/items/{id}
        [HttpDelete("{id}")]
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
}
