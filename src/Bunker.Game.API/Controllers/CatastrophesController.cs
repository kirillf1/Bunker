using Ardalis.Result;
using Ardalis.Result.AspNetCore;
using Bunker.Application.Shared.CQRS;
using Bunker.Game.Application.Queries.Catastrophes;
using Microsoft.AspNetCore.Mvc;

namespace Bunker.Game.API.Controllers;

[ApiController]
[Route("api/game-sessions")]
public class CatastrophesController : ControllerBase
{
    private readonly IQueryHandler<GetCatastropheQuery, Result<CatastropheDto>> _getCatastropheQueryHandler;

    public CatastrophesController(IQueryHandler<GetCatastropheQuery, Result<CatastropheDto>> getCatastropheQueryHandler)
    {
        _getCatastropheQueryHandler = getCatastropheQueryHandler;
    }

    [HttpGet("{gameSessionId:guid}/catastrophe")]
    [TranslateResultToActionResult]
    public async Task<Result<CatastropheDto>> GetCatastrophe(Guid gameSessionId)
    {
        var query = new GetCatastropheQuery { GameSessionId = gameSessionId };
        return await _getCatastropheQueryHandler.Handle(query, CancellationToken.None);
    }
} 