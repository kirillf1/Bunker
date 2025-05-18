using Ardalis.Result;
using Ardalis.Result.AspNetCore;
using Bunker.Application.Shared.CQRS;
using Bunker.Game.Application.Queries.Bunkers;
using Microsoft.AspNetCore.Mvc;

namespace Bunker.Game.API.Controllers;

[ApiController]
[Route("api/game-sessions")]
public class BunkersController : ControllerBase
{
    private readonly IQueryHandler<GetBunkerQuery, Result<BunkerDto>> _getBunkerQueryHandler;

    public BunkersController(IQueryHandler<GetBunkerQuery, Result<BunkerDto>> getBunkerQueryHandler)
    {
        _getBunkerQueryHandler = getBunkerQueryHandler;
    }

    [HttpGet("{gameSessionId:guid}/bunker")]
    [TranslateResultToActionResult]
    public async Task<Result<BunkerDto>> GetBunker(Guid gameSessionId)
    {
        var query = new GetBunkerQuery { GameSessionId = gameSessionId };

        return await _getBunkerQueryHandler.Handle(query, CancellationToken.None);
    }
}
