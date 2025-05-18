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
    private readonly ILogger<CharactersController> _logger;

    public CharactersController(
        IQueryHandler<GetCharacterQuery, Result<CharacterDto>> getCharacterQueryHandler,
        IQueryHandler<
            GetCharactersByGameSessionQuery,
            Result<IEnumerable<CharacterDto>>
        > getCharactersByGameSessionQueryHandler,
        ILogger<CharactersController> logger
    )
    {
        _getCharacterQueryHandler = getCharacterQueryHandler;
        _getCharactersByGameSessionQueryHandler = getCharactersByGameSessionQueryHandler;
        _logger = logger;
    }

    [HttpGet("characters/{characterId:guid}")]
    [TranslateResultToActionResult]
    public async Task<Result<CharacterDto>> GetCharacter(Guid characterId, CancellationToken cancellationToken)
    {
        using (_logger.BeginScope(new Dictionary<string, object> { ["CharacterId"] = characterId }))
        {
            var query = new GetCharacterQuery { CharacterId = characterId };

            var result = await _getCharacterQueryHandler.Handle(query, cancellationToken);

            return result;
        }
    }

    [HttpGet("game-sessions/{gameSessionId:guid}/characters")]
    [TranslateResultToActionResult]
    public async Task<Result<IEnumerable<CharacterDto>>> GetCharactersByGameSession(
        Guid gameSessionId,
        CancellationToken cancellationToken
    )
    {
        using (_logger.BeginScope(new Dictionary<string, object> { ["GameSessionId"] = gameSessionId }))
        {
            var query = new GetCharactersByGameSessionQuery { GameSessionId = gameSessionId };

            var result = await _getCharactersByGameSessionQueryHandler.Handle(query, cancellationToken);

            return result;
        }
    }
}
