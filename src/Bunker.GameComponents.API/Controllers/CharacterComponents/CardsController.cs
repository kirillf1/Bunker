using Bunker.GameComponents.API.Entities.CharacterComponents.Cards;
using Bunker.GameComponents.API.Infrastructure.Database;
using Bunker.GameComponents.API.Models.CharacterComponents.Cards;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Bunker.GameComponents.API.Controllers.CharacterComponents;

[Route("api/cards")]
[ApiController]
[Produces("application/json")]
public class CardsController : ControllerBase
{
    private readonly GameComponentsContext _context;

    public CardsController(GameComponentsContext context)
    {
        _context = context;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<CardDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<IEnumerable<CardDto>>> GetAll()
    {
        var cards = await _context
            .Cards.Select(c => new CardDto
            {
                Id = c.Id,
                Description = c.Description,
                CardAction = c.CardAction,
            })
            .ToListAsync();

        return Ok(cards);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(CardDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<CardDto>> GetById(Guid id)
    {
        var card = await _context.Cards.FirstOrDefaultAsync(c => c.Id == id);

        if (card is null)
        {
            return NotFound();
        }

        var dto = new CardDto
        {
            Id = card.Id,
            Description = card.Description,
            CardAction = card.CardAction,
        };

        return Ok(dto);
    }

    [HttpPost]
    [ProducesResponseType(typeof(CardDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<CardDto>> Create([FromBody] CardCreateDto dto)
    {
        var card = new CardEntity(dto.Description, dto.CardAction);

        _context.Cards.Add(card);
        await _context.SaveChangesAsync();

        var resultDto = new CardDto
        {
            Id = card.Id,
            Description = card.Description,
            CardAction = card.CardAction,
        };

        return CreatedAtAction(nameof(GetById), new { id = card.Id }, resultDto);
    }

    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Update(Guid id, [FromBody] CardUpdateDto dto)
    {
        var card = await _context.Cards.FirstOrDefaultAsync(c => c.Id == id);

        if (card is null)
        {
            return NotFound();
        }

        card.Description = dto.Description;
        card.CardAction = dto.CardAction;

        _context.Update(card);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Delete(Guid id)
    {
        var card = await _context.Cards.FirstOrDefaultAsync(c => c.Id == id);

        if (card is null)
        {
            return NotFound();
        }

        _context.Cards.Remove(card);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
