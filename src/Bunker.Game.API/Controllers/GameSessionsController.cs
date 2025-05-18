using Ardalis.Result;
using Ardalis.Result.AspNetCore;
using Bunker.Application.Shared.CQRS;
using Bunker.Game.Application.Commands.GameSessions.CreateGameSession;
using Bunker.Game.Application.Queries.GameSessions;
using Microsoft.AspNetCore.Mvc;

namespace Bunker.Game.API.Controllers;

[ApiController]
[Route("api/game-sessions")]
public class GameSessionsController : ControllerBase
{
    private readonly ICommandHandler<
        CreateGameSessionCommand,
        Result<GameSessionCreationResult>
    > _createGameSessionCommandHandler;
    private readonly IQueryHandler<GetGameSessionQuery, Result<GameSessionDto>> _getGameSessionQueryHandler;

    public GameSessionsController(
        ICommandHandler<CreateGameSessionCommand, Result<GameSessionCreationResult>> createGameSessionCommandHandler,
        IQueryHandler<GetGameSessionQuery, Result<GameSessionDto>> getGameSessionQueryHandler
    )
    {
        _createGameSessionCommandHandler = createGameSessionCommandHandler;
        _getGameSessionQueryHandler = getGameSessionQueryHandler;
    }

    [HttpPost]
    [TranslateResultToActionResult]
    public async Task<Result<GameSessionCreationResult>> CreateGameSession([FromBody] CreateGameSessionCommand request)
    {
        return await _createGameSessionCommandHandler.Handle(request, CancellationToken.None);
    }

    [HttpGet("{gameSessionId:guid}")]
    [TranslateResultToActionResult]
    public async Task<Result<GameSessionDto>> GetGameSession(Guid gameSessionId)
    {
        var query = new GetGameSessionQuery { GameSessionId = gameSessionId };
        return await _getGameSessionQueryHandler.Handle(query, CancellationToken.None);
    }
}
