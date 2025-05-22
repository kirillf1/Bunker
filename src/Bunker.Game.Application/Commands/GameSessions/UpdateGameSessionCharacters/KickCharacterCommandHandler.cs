using Ardalis.Result;
using Bunker.Application.Shared.CQRS;
using Bunker.Game.Domain.AggregateModels.GameSessions;
using Microsoft.Extensions.Logging;

namespace Bunker.Game.Application.Commands.GameSessions.UpdateGameSessionCharacters;

public class KickCharacterCommandHandler : ICommandHandler<KickCharacterCommand, Result>
{
    private readonly IGameSessionRepository _gameSessionRepository;
    private readonly ILogger<KickCharacterCommandHandler> _logger;

    public KickCharacterCommandHandler(
        IGameSessionRepository gameSessionRepository,
        ILogger<KickCharacterCommandHandler> logger
    )
    {
        _gameSessionRepository = gameSessionRepository;
        _logger = logger;
    }

    public async Task<Result> Handle(KickCharacterCommand command, CancellationToken cancellation)
    {
        var gameSession = await _gameSessionRepository.GetGameSession(command.GameSessionId);

        if (gameSession is null)
        {
            _logger.LogWarning(
                "Game session {GameSessionId} not found during attempt to kick character {CharacterId}",
                command.GameSessionId,
                command.CharacterId
            );
            return Result.NotFound("Game session not found");
        }

        try
        {
            gameSession.KickCharacter(command.CharacterId);

            await _gameSessionRepository.Update(gameSession);

            await _gameSessionRepository.UnitOfWork.SaveChangesAsync(cancellation);

            _logger.LogInformation(
                "Character {CharacterId} successfully kicked from game session {GameSessionId}. Game state: {GameState}",
                command.CharacterId,
                command.GameSessionId,
                gameSession.GameState.ToString()
            );

            _logger.LogInformation(
                "After kicking character {CharacterId}, game session {GameSessionId} has {RemainingCharacters} active characters",
                command.CharacterId,
                command.GameSessionId,
                gameSession.Characters.Count(c => !c.IsKicked)
            );

            return Result.Success();
        }
        catch (InvalidGameOperationException ex)
        {
            _logger.LogError(
                ex,
                "Failed to kick character {CharacterId} from game session {GameSessionId}: {ErrorMessage}",
                command.CharacterId,
                command.GameSessionId,
                ex.Message
            );
            return Result.Invalid(new ValidationError(ex.Message));
        }
        catch (ArgumentException ex)
        {
            _logger.LogError(
                ex,
                "Failed to kick character {CharacterId} from game session {GameSessionId}: {ErrorMessage}",
                command.CharacterId,
                command.GameSessionId,
                ex.Message
            );
            return Result.Invalid(new ValidationError(ex.Message));
        }
    }
}
