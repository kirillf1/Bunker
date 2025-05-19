using Ardalis.Result;
using Ardalis.Result.AspNetCore;
using Bunker.Application.Shared.CQRS;
using Bunker.Game.API.Models.GameSessions;
using Bunker.Game.Application.Commands.GameSessions.CreateGameSession;
using Bunker.Game.Application.Commands.GameSessions.UpdateGameSessionCharacters;
using Bunker.Game.Application.Commands.GameSessions.UpdateGameSessionState;
using Bunker.Game.Application.Queries.GameSessions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

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
    private readonly ICommandHandler<OccupyCharacterCommand, Result<Guid>> _occupyCharacterCommandHandler;
    private readonly ICommandHandler<KickCharacterCommand, Result> _kickCharacterCommandHandler;
    private readonly ICommandHandler<EndGameSessionCommand, Result> _endGameSessionCommandHandler;
    private readonly ICommandHandler<TerminateGameSessionCommand, Result> _terminateGameSessionCommandHandler;
    private readonly ILogger<GameSessionsController> _logger;

    public GameSessionsController(
        ICommandHandler<CreateGameSessionCommand, Result<GameSessionCreationResult>> createGameSessionCommandHandler,
        IQueryHandler<GetGameSessionQuery, Result<GameSessionDto>> getGameSessionQueryHandler,
        ICommandHandler<OccupyCharacterCommand, Result<Guid>> occupyCharacterCommandHandler,
        ICommandHandler<KickCharacterCommand, Result> kickCharacterCommandHandler,
        ICommandHandler<EndGameSessionCommand, Result> endGameSessionCommandHandler,
        ICommandHandler<TerminateGameSessionCommand, Result> terminateGameSessionCommandHandler,
        ILogger<GameSessionsController> logger
    )
    {
        _createGameSessionCommandHandler = createGameSessionCommandHandler;
        _getGameSessionQueryHandler = getGameSessionQueryHandler;
        _occupyCharacterCommandHandler = occupyCharacterCommandHandler;
        _kickCharacterCommandHandler = kickCharacterCommandHandler;
        _endGameSessionCommandHandler = endGameSessionCommandHandler;
        _terminateGameSessionCommandHandler = terminateGameSessionCommandHandler;
        _logger = logger;
    }

    [HttpPost]
    [TranslateResultToActionResult]
    public async Task<Result<GameSessionCreationResult>> CreateGameSession(
        [FromBody] CreateGameSessionRequest request,
        CancellationToken cancellationToken
    )
    {
        using (
            _logger.BeginScope(
                new Dictionary<string, object> { ["PlayerName"] = request.PlayerName, ["PlayerId"] = request.PlayerId }
            )
        )
        {
            var command = new CreateGameSessionCommand(
                Guid.CreateVersion7(),
                request.Name,
                request.PlayerId,
                request.PlayerName,
                request.CharactersCount
            );

            var result = await _createGameSessionCommandHandler.Handle(command, cancellationToken);

            return result;
        }
    }

    [HttpGet("{gameSessionId:guid}")]
    [TranslateResultToActionResult]
    public async Task<Result<GameSessionDto>> GetGameSession(Guid gameSessionId, CancellationToken cancellationToken)
    {
        using (_logger.BeginScope(new Dictionary<string, object> { ["GameSessionId"] = gameSessionId }))
        {
            var query = new GetGameSessionQuery { GameSessionId = gameSessionId };

            var result = await _getGameSessionQueryHandler.Handle(query, cancellationToken);

            return result;
        }
    }

    [HttpPost("{gameSessionId:guid}/occupy-character")]
    [TranslateResultToActionResult]
    public async Task<Result<Guid>> OccupyCharacter(
        Guid gameSessionId,
        [FromBody] OccupyCharacterRequest request,
        CancellationToken cancellationToken
    )
    {
        using (
            _logger.BeginScope(
                new Dictionary<string, object>
                {
                    ["GameSessionId"] = gameSessionId,
                    ["PlayerId"] = request.PlayerId,
                    ["PlayerName"] = request.PlayerName,
                }
            )
        )
        {
            var command = new OccupyCharacterCommand(gameSessionId, request.PlayerId, request.PlayerName);

            var result = await _occupyCharacterCommandHandler.Handle(command, cancellationToken);

            return result;
        }
    }

    [HttpPost("{gameSessionId:guid}/characters/{characterId:guid}/kick")]
    [TranslateResultToActionResult]
    public async Task<Result> KickCharacter(Guid gameSessionId, Guid characterId, CancellationToken cancellationToken)
    {
        using (
            _logger.BeginScope(
                new Dictionary<string, object> { ["GameSessionId"] = gameSessionId, ["CharacterId"] = characterId }
            )
        )
        {
            var command = new KickCharacterCommand(gameSessionId, characterId);

            var result = await _kickCharacterCommandHandler.Handle(command, cancellationToken);

            return result;
        }
    }

    [HttpPost("{gameSessionId:guid}/terminate")]
    [TranslateResultToActionResult]
    public async Task<Result> TerminateGameSession(Guid gameSessionId, CancellationToken cancellationToken)
    {
        using (_logger.BeginScope(new Dictionary<string, object> { ["GameSessionId"] = gameSessionId }))
        {
            var command = new TerminateGameSessionCommand(gameSessionId);

            var result = await _terminateGameSessionCommandHandler.Handle(command, cancellationToken);

            return result;
        }
    }
}
