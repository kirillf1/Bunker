using Ardalis.Result;
using Ardalis.Result.AspNetCore;
using Bunker.Application.Shared.CQRS;
using Bunker.Game.Application.Queries.Characters;
using Bunker.Game.Application.Queries.Characters.Models;
using Microsoft.AspNetCore.Mvc;

namespace Bunker.Game.API.Controllers;

[ApiController]
[Route("api")]
public class CharactersController : ControllerBase
{
    private readonly IQueryHandler<GetCharacterQuery, Result<CharacterDto>> _getCharacterQueryHandler;
    private readonly IQueryHandler<
        GetCharactersByGameSessionQuery,
        Result<IEnumerable<CharacterDto>>
    > _getCharactersByGameSessionQueryHandler;

    public CharactersController(
        IQueryHandler<GetCharacterQuery, Result<CharacterDto>> getCharacterQueryHandler,
        IQueryHandler<
            GetCharactersByGameSessionQuery,
            Result<IEnumerable<CharacterDto>>
        > getCharactersByGameSessionQueryHandler
    )
    {
        _getCharacterQueryHandler = getCharacterQueryHandler;
        _getCharactersByGameSessionQueryHandler = getCharactersByGameSessionQueryHandler;
    }

    [HttpGet("characters/{characterId:guid}")]
    [TranslateResultToActionResult]
    public async Task<Result<CharacterDto>> GetCharacter(Guid characterId)
    {
        var query = new GetCharacterQuery { CharacterId = characterId };
        return await _getCharacterQueryHandler.Handle(query, CancellationToken.None);
    }

    [HttpGet("game-sessions/{gameSessionId:guid}/characters")]
    [TranslateResultToActionResult]
    public async Task<Result<IEnumerable<CharacterDto>>> GetCharactersByGameSession(Guid gameSessionId)
    {
        var query = new GetCharactersByGameSessionQuery { GameSessionId = gameSessionId };
        return await _getCharactersByGameSessionQueryHandler.Handle(query, CancellationToken.None);
    }
}
