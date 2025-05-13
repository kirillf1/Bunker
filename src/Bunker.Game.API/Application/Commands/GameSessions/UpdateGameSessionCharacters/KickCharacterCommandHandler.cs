using Ardalis.Result;
using Bunker.Application.Shared.CQRS;
using Bunker.Domain.Shared.Exceptions;
using Bunker.Game.Domain.AggregateModels.GameSessions;

namespace Bunker.Game.API.Application.Commands.GameSessions.UpdateGameSessionCharacters;

public class KickCharacterCommandHandler : ICommandHandler<KickCharacterCommand, Result>
{
    private readonly IGameSessionRepository _gameSessionRepository;

    public KickCharacterCommandHandler(IGameSessionRepository gameSessionRepository)
    {
        _gameSessionRepository = gameSessionRepository;
    }

    public async Task<Result> Handle(KickCharacterCommand command, CancellationToken cancellation)
    {
        var gameSession = await _gameSessionRepository.GetGameSession(command.GameSessionId);

        if (gameSession is null)
            return Result.NotFound("Game session not found");

        try
        {
            gameSession.KickCharacter(command.CharacterId);

            await _gameSessionRepository.Update(gameSession);

            await _gameSessionRepository.UnitOfWork.SaveChangesAsync(cancellation);

            return Result.Success();
        }
        catch (InvalidGameOperationException ex)
        {
            return Result.Error(ex.Message);
        }
        catch (ArgumentException ex)
        {
            return Result.Error(ex.Message);
        }
    }
}
