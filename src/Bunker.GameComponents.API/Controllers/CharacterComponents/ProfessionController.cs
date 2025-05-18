using Bunker.GameComponents.API.Entities.CharacterComponents;
using Bunker.GameComponents.API.Infrastructure.Database;
using Bunker.GameComponents.API.Models.CharacterComponents.Profession;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Bunker.GameComponents.API.Controllers.CharacterComponents;

[Route("api/character-components/professions")]
[ApiController]
[Produces("application/json")]
public class ProfessionController : ControllerBase
{
    private readonly GameComponentsContext _context;
    private readonly ILogger<ProfessionController> _logger;

    public ProfessionController(
        GameComponentsContext context,
        ILogger<ProfessionController> logger)
    {
        _context = context;
        _logger = logger;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<ProfessionDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<IEnumerable<ProfessionDto>>> GetAll(CancellationToken cancellationToken)
    {
        using (_logger.BeginScope(new Dictionary<string, object>
        {
            ["Operation"] = "GetAllProfessions"
        }))
        {
            _logger.LogInformation("Получение списка всех профессий");
            
            var professions = await _context
                .Professions.Select(p => new ProfessionDto { Id = p.Id, Description = p.Description })
                .ToListAsync(cancellationToken);
            
            _logger.LogInformation("Получено {Count} профессий", professions.Count);
            
            return Ok(professions);
        }
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ProfessionDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ProfessionDto>> GetById(Guid id, CancellationToken cancellationToken)
    {
        using (_logger.BeginScope(new Dictionary<string, object>
        {
            ["ProfessionId"] = id
        }))
        {
            _logger.LogInformation("Получение профессии по ID");
            
            var profession = await _context.Professions.FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
            if (profession is null)
            {
                _logger.LogWarning("Профессия не найдена");
                return NotFound();
            }
            
            var dto = new ProfessionDto { Id = profession.Id, Description = profession.Description };
            
            _logger.LogInformation("Профессия найдена: {Description}", profession.Description);
            
            return Ok(dto);
        }
    }

    [HttpPost]
    [ProducesResponseType(typeof(ProfessionDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ProfessionDto>> Create([FromBody] CreateProfessionDto dto, CancellationToken cancellationToken)
    {
        using (_logger.BeginScope(new Dictionary<string, object>
        {
            ["Operation"] = "CreateProfession"
        }))
        {
            _logger.LogInformation("Создание новой профессии с описанием: {Description}", dto.Description);
            
            var profession = new ProfessionEntity(dto.Description);
            _context.Professions.Add(profession);
            await _context.SaveChangesAsync(cancellationToken);
            
            _logger.LogInformation("Создана новая профессия с ID: {ProfessionId}", profession.Id);
            
            var resultDto = new ProfessionDto { Id = profession.Id, Description = profession.Description };
            return CreatedAtAction(nameof(GetById), new { id = profession.Id }, resultDto);
        }
    }

    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateProfessionDto dto, CancellationToken cancellationToken)
    {
        using (_logger.BeginScope(new Dictionary<string, object>
        {
            ["ProfessionId"] = id
        }))
        {
            _logger.LogInformation("Обновление профессии");
            
            var profession = await _context.Professions.FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
            if (profession is null)
            {
                _logger.LogWarning("Профессия не найдена");
                return NotFound();
            }
            
            _logger.LogInformation("Обновление описания с '{OldDescription}' на '{NewDescription}'", 
                profession.Description, dto.Description);
            
            profession.Description = dto.Description;
            _context.Update(profession);
            await _context.SaveChangesAsync(cancellationToken);
            
            _logger.LogInformation("Профессия успешно обновлена");
            
            return NoContent();
        }
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        using (_logger.BeginScope(new Dictionary<string, object>
        {
            ["ProfessionId"] = id
        }))
        {
            _logger.LogInformation("Удаление профессии");
            
            var profession = await _context.Professions.FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
            if (profession is null)
            {
                _logger.LogWarning("Профессия не найдена");
                return NotFound();
            }
            
            _logger.LogInformation("Удаление профессии с описанием: {Description}", profession.Description);
            
            _context.Professions.Remove(profession);
            await _context.SaveChangesAsync(cancellationToken);
            
            _logger.LogInformation("Профессия успешно удалена");
            
            return NoContent();
        }
    }
}
