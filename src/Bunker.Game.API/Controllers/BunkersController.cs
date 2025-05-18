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
    private readonly ILogger<BunkersController> _logger;

    public BunkersController(
        IQueryHandler<GetBunkerQuery, Result<BunkerDto>> getBunkerQueryHandler,
        ILogger<BunkersController> logger
    )
    {
        _getBunkerQueryHandler = getBunkerQueryHandler;
        _logger = logger;
    }

    [HttpGet("{gameSessionId:guid}/bunker")]
    [TranslateResultToActionResult]
    public async Task<Result<BunkerDto>> GetBunker(Guid gameSessionId, CancellationToken cancellationToken)
    {
        using (_logger.BeginScope(new Dictionary<string, object> { ["GameSessionId"] = gameSessionId }))
        {
            var query = new GetBunkerQuery { GameSessionId = gameSessionId };

            var result = await _getBunkerQueryHandler.Handle(query, cancellationToken);

            return result;
        }
    }
}
