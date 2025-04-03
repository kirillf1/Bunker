using Bunker.GameComponents.API.Entities.CharacterComponents;
using Bunker.GameComponents.API.Infrastructure;
using Bunker.GameComponents.API.Models.CharacterComponents.AdditionalInformation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Bunker.GameComponents.API.Controllers.CharacterComponents;

[Route("api/character-components/additional-information")]
[ApiController]
[Produces("application/json")]
public class AdditionalInformationController : ControllerBase
{
    private readonly GameComponentsContext _context;

    public AdditionalInformationController(GameComponentsContext context)
    {
        _context = context;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<AdditionalInformationDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<IEnumerable<AdditionalInformationDto>>> GetAll()
    {
        var additionalInformations = await _context
            .AdditionalInformationEntitles.Select(p => new AdditionalInformationDto
            {
                Id = p.Id,
                Description = p.Description,
            })
            .ToListAsync();

        return Ok(additionalInformations);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(AdditionalInformationDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<AdditionalInformationDto>> GetById(Guid id)
    {
        var additionalInformation = await _context.AdditionalInformationEntitles.FirstOrDefaultAsync(p => p.Id == id);
        if (additionalInformation is null)
        {
            return NotFound();
        }

        var dto = new AdditionalInformationDto
        {
            Id = additionalInformation.Id,
            Description = additionalInformation.Description,
        };
        return Ok(dto);
    }

    [HttpPost]
    [ProducesResponseType(typeof(AdditionalInformationDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<AdditionalInformationDto>> Create([FromBody] CreateAdditionalInformationDto dto)
    {
        var additionalInformation = new AdditionalInformationEntity(dto.Description);
        _context.AdditionalInformationEntitles.Add(additionalInformation);
        await _context.SaveChangesAsync();

        var resultDto = new AdditionalInformationDto
        {
            Id = additionalInformation.Id,
            Description = additionalInformation.Description,
        };
        return CreatedAtAction(nameof(GetById), new { id = additionalInformation.Id }, resultDto);
    }

    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateAdditionalInformationDto dto)
    {
        var additionalInformation = await _context.AdditionalInformationEntitles.FirstOrDefaultAsync(p => p.Id == id);
        if (additionalInformation is null)
        {
            return NotFound();
        }

        additionalInformation.Description = dto.Description;
        _context.Update(additionalInformation);
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
        var additionalInformation = await _context.AdditionalInformationEntitles.FirstOrDefaultAsync(p => p.Id == id);
        if (additionalInformation is null)
        {
            return NotFound();
        }

        _context.AdditionalInformationEntitles.Remove(additionalInformation);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
