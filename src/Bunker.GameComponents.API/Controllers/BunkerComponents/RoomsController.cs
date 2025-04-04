using Bunker.GameComponents.API.Entities.BunkerComponents;
using Bunker.GameComponents.API.Infrastructure.Database;
using Bunker.GameComponents.API.Models.BunkerComponents.Rooms;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Bunker.GameComponents.API.Controllers.BunkerComponents;

[Route("api/bunker-components/rooms")]
[ApiController]
[Produces("application/json")]
public class RoomsController : ControllerBase
{
    private readonly GameComponentsContext _context;

    public RoomsController(GameComponentsContext context)
    {
        _context = context;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<RoomDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<IEnumerable<RoomDto>>> GetRooms()
    {
        var rooms = await _context
            .BunkerRooms.Select(r => new RoomDto { Id = r.Id, Description = r.Description })
            .ToListAsync();
        return Ok(rooms);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(RoomDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<RoomDto>> GetRoom(Guid id)
    {
        var room = await _context.BunkerRooms.FirstOrDefaultAsync(x => x.Id == id);
        if (room is null)
        {
            return NotFound();
        }

        var dto = new RoomDto { Id = room.Id, Description = room.Description };
        return Ok(dto);
    }

    [HttpPost]
    [ProducesResponseType(typeof(RoomDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<RoomDto>> CreateRoom([FromBody] CreateRoomDto dto)
    {
        var room = new RoomEntity(dto.Description);
        _context.BunkerRooms.Add(room);
        await _context.SaveChangesAsync();

        var resultDto = new RoomDto { Id = room.Id, Description = room.Description };

        return CreatedAtAction(nameof(GetRoom), new { id = room.Id }, resultDto);
    }

    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateRoom(Guid id, [FromBody] UpdateRoomDto dto)
    {
        var room = await _context.BunkerRooms.FirstOrDefaultAsync(x => x.Id == id);
        if (room is null)
        {
            return NotFound();
        }

        room.Description = dto.Description;

        _context.Update(room);

        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteRoom(Guid id)
    {
        var room = await _context.BunkerRooms.FirstOrDefaultAsync(x => x.Id == id);
        if (room is null)
        {
            return NotFound();
        }

        _context.BunkerRooms.Remove(room);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
