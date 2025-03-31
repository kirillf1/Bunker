using Bunker.GameComponents.API.Entities.BunkerComponents;
using Bunker.GameComponents.API.Infrastructure;
using Bunker.GameComponents.API.Models.BunkerComponents.Environments;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Bunker.GameComponents.API.Controllers.BunkerComponents
{
    [Route("api/bunker-components/environments")]
    [ApiController]
    public class EnvironmentsController : ControllerBase
    {
        private readonly GameComponentsContext _context;

        public EnvironmentsController(GameComponentsContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<EnvironmentDto>>> GetEnvironments()
        {
            var environments = await _context
                .BunkerEnvironments.Select(e => new EnvironmentDto { Id = e.Id, Description = e.Description })
                .ToListAsync();
            return Ok(environments);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<EnvironmentDto>> GetEnvironment(Guid id)
        {
            var environment = await _context.BunkerEnvironments.FirstOrDefaultAsync(x => x.Id == id);
            if (environment is null)
            {
                return NotFound();
            }

            var dto = new EnvironmentDto { Id = environment.Id, Description = environment.Description };
            return Ok(dto);
        }

        [HttpPost]
        public async Task<ActionResult<EnvironmentDto>> CreateEnvironment([FromBody] CreateEnvironmentDto dto)
        {
            var environment = new EnvironmentEntity(dto.Description);

            await _context.BunkerEnvironments.AddAsync(environment);

            await _context.SaveChangesAsync();

            var resultDto = new EnvironmentDto { Id = environment.Id, Description = environment.Description };

            return CreatedAtAction(nameof(GetEnvironment), new { id = environment.Id }, resultDto);
        }

        // PUT: api/bunker-components/environments/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEnvironment(Guid id, [FromBody] UpdateEnvironmentDto dto)
        {
            var environment = await _context.BunkerEnvironments.FirstOrDefaultAsync(x => x.Id == id);
            if (environment is null)
            {
                return NotFound();
            }

            environment.Description = dto.Description;
            _context.Update(environment);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEnvironment(Guid id)
        {
            var environment = await _context.BunkerEnvironments.FirstOrDefaultAsync(x => x.Id == id);
            if (environment is null)
            {
                return NotFound();
            }

            _context.BunkerEnvironments.Remove(environment);

            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
