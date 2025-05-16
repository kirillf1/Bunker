using Ardalis.Result;
using Bunker.Application.Shared.CQRS;
using Bunker.Game.Domain.AggregateModels.GameSessions;

namespace Bunker.Game.Application.Commands.GameSessions.UpdateGameSessionState;

public class TerminateGameSessionCommandHandler : ICommandHandler<TerminateGameSessionCommand, Result>
{
    private readonly IGameSessionRepository _gameSessionRepository;

    public TerminateGameSessionCommandHandler(IGameSessionRepository gameSessionRepository)
    {
        _gameSessionRepository = gameSessionRepository;
    }

    public async Task<Result> Handle(TerminateGameSessionCommand command, CancellationToken cancellation)
    {
        var gameSession = await _gameSessionRepository.GetGameSession(command.GameSessionId);

        if (gameSession is null)
            return Result.NotFound("Game session not found");

        try
        {
            gameSession.TerminateGame();

            await _gameSessionRepository.Update(gameSession);

            await _gameSessionRepository.UnitOfWork.SaveChangesAsync(cancellation);

            return Result.Success();
        }
        catch (InvalidGameOperationException ex)
        {
            return Result.Error(ex.Message);
        }
    }
}
