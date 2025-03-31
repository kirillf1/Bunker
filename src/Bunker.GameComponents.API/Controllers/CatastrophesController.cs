using Bunker.GameComponents.API.Entities.CatastropheComponents;
using Bunker.GameComponents.API.Infrastructure;
using Bunker.GameComponents.API.Models.Catastrophes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Bunker.GameComponents.API.Controllers
{
    [Route("api/catastrophes")]
    [ApiController]
    public class CatastrophesController : ControllerBase
    {
        private readonly GameComponentsContext _context;

        public CatastrophesController(GameComponentsContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CatastropheDto>>> GetCatastrophes()
        {
            var catastrophes = await _context
                .Catastrophes.Select(c => new CatastropheDto { Id = c.Id, Description = c.Description })
                .ToListAsync();

            return Ok(catastrophes);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CatastropheDto>> GetCatastrophe(Guid id)
        {
            var catastrophe = await _context.Catastrophes.FirstOrDefaultAsync(x => x.Id == id);
            if (catastrophe is null)
            {
                return NotFound();
            }

            var dto = new CatastropheDto { Id = catastrophe.Id, Description = catastrophe.Description };

            return Ok(dto);
        }

        [HttpPost]
        public async Task<ActionResult<CatastropheDto>> CreateCatastrophe([FromBody] CreateCatastropheDto dto)
        {
            var catastrophe = new CatastropheEntity(dto.Description);

            await _context.Catastrophes.AddAsync(catastrophe);

            await _context.SaveChangesAsync();

            var resultDto = new CatastropheDto { Id = catastrophe.Id, Description = catastrophe.Description };

            return CreatedAtAction(nameof(GetCatastrophe), new { id = catastrophe.Id }, resultDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCatastrophe(Guid id, [FromBody] UpdateCatastropheDto dto)
        {
            var catastrophe = await _context.Catastrophes.FirstOrDefaultAsync(x => x.Id == id);
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
        public async Task<IActionResult> DeleteCatastrophe(Guid id)
        {
            var catastrophe = await _context.Catastrophes.FirstOrDefaultAsync(x => x.Id == id);
            if (catastrophe is null)
            {
                return NotFound();
            }

            _context.Catastrophes.Remove(catastrophe);

            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
