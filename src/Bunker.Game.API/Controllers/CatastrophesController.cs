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
    private readonly ILogger<CatastrophesController> _logger;

    public CatastrophesController(
        IQueryHandler<GetCatastropheQuery, Result<CatastropheDto>> getCatastropheQueryHandler,
        ILogger<CatastrophesController> logger
    )
    {
        _getCatastropheQueryHandler = getCatastropheQueryHandler;
        _logger = logger;
    }

    [HttpGet("{gameSessionId:guid}/catastrophe")]
    [TranslateResultToActionResult]
    public async Task<Result<CatastropheDto>> GetCatastrophe(Guid gameSessionId, CancellationToken cancellationToken)
    {
        using (_logger.BeginScope(new Dictionary<string, object> { ["GameSessionId"] = gameSessionId }))
        {
            var query = new GetCatastropheQuery { GameSessionId = gameSessionId };

            var result = await _getCatastropheQueryHandler.Handle(query, cancellationToken);

            return result;
        }
    }
}
