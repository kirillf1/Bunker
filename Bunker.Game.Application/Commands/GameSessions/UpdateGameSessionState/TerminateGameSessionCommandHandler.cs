using Ardalis.Result;
using Bunker.Application.Shared.CQRS;
using Bunker.Game.Domain.AggregateModels.GameSessions;

namespace Bunker.Game.Application.Commands.GameSessions.UpdateGameSessionState;

public class TerminateGameSessionCommandHandler : ICommandHandler<TerminateGameSessionCommand, Result>
{
    private readonly IGameSessionRepository _gameSessionRepository;
    private readonly ILogger<TerminateGameSessionCommandHandler> _logger;

    public TerminateGameSessionCommandHandler(
        IGameSessionRepository gameSessionRepository,
        ILogger<TerminateGameSessionCommandHandler> logger
    )
    {
        _gameSessionRepository = gameSessionRepository;
        _logger = logger;
    }

    public async Task<Result> Handle(TerminateGameSessionCommand command, CancellationToken cancellation)
    {
        var gameSession = await _gameSessionRepository.GetGameSession(command.GameSessionId);

        if (gameSession is null)
        {
            _logger.LogWarning(
                "Game session {GameSessionId} not found during termination attempt",
                command.GameSessionId
            );
            return Result.NotFound("Game session not found");
        }

        try
        {
            gameSession.TerminateGame();

            await _gameSessionRepository.Update(gameSession);

            await _gameSessionRepository.UnitOfWork.SaveChangesAsync(cancellation);

            _logger.LogInformation(
                "Game session {GameSessionId} successfully terminated. Characters count: {CharactersCount}, Game name: {GameName}",
                command.GameSessionId,
                gameSession.Characters.Count,
                gameSession.Name
            );

            return Result.Success();
        }
        catch (InvalidGameOperationException ex)
        {
            _logger.LogError(
                ex,
                "Failed to terminate game session {GameSessionId}: {ErrorMessage}",
                command.GameSessionId,
                ex.Message
            );
            return Result.Invalid(new ValidationError(ex.Message));
        }
    }
}
