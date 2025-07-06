using Ardalis.Result;
using Bunker.Application.Shared.CQRS;
using Bunker.Game.Domain.AggregateModels.GameSessions;

namespace Bunker.Game.Application.Commands.GameSessions.UpdateGameSessionState;

public class EndGameSessionCommandHandler : ICommandHandler<EndGameSessionCommand, Result>
{
    private readonly IGameSessionRepository _gameSessionRepository;
    private readonly ILogger<EndGameSessionCommandHandler> _logger;

    public EndGameSessionCommandHandler(
        IGameSessionRepository gameSessionRepository,
        ILogger<EndGameSessionCommandHandler> logger
    )
    {
        _gameSessionRepository = gameSessionRepository;
        _logger = logger;
    }

    public async Task<Result> Handle(EndGameSessionCommand command, CancellationToken cancellation)
    {
        var gameSession = await _gameSessionRepository.GetGameSession(command.GameSessionId);

        if (gameSession is null)
        {
            _logger.LogWarning(
                "Game session {GameSessionId} not found during attempt to end it",
                command.GameSessionId
            );
            return Result.NotFound("Game session not found");
        }

        try
        {
            gameSession.SetGameResult(command.GameResultDescription);

            await _gameSessionRepository.Update(gameSession);

            await _gameSessionRepository.UnitOfWork.SaveChangesAsync(cancellation);

            _logger.LogInformation("Game session {GameSessionId} successfully ended", command.GameSessionId);

            return Result.Success();
        }
        catch (InvalidGameOperationException ex)
        {
            _logger.LogError(
                ex,
                "Failed to end game session {GameSessionId}: {ErrorMessage}",
                command.GameSessionId,
                ex.Message
            );
            return Result.Invalid(new ValidationError(ex.Message));
        }
    }
}
