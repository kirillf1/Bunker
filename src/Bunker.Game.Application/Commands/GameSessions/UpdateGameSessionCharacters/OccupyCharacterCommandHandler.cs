using Ardalis.Result;
using Bunker.Application.Shared.CQRS;
using Bunker.Game.Domain.AggregateModels.GameSessions;

namespace Bunker.Game.Application.Commands.GameSessions.UpdateGameSessionCharacters;

public class OccupyCharacterCommandHandler : ICommandHandler<OccupyCharacterCommand, Result<Guid>>
{
    private readonly IGameSessionRepository _gameSessionRepository;

    public OccupyCharacterCommandHandler(IGameSessionRepository gameSessionRepository)
    {
        _gameSessionRepository = gameSessionRepository;
    }

    public async Task<Result<Guid>> Handle(OccupyCharacterCommand command, CancellationToken cancellation)
    {
        var gameSession = await _gameSessionRepository.GetGameSession(command.GameSessionId);

        if (gameSession is null)
            return Result<Guid>.NotFound("Game session not found");

        var player = new Player(command.PlayerId, command.PlayerName);

        try
        {
            var character = gameSession.OccupyCharacter(player);

            await _gameSessionRepository.Update(gameSession);

            await _gameSessionRepository.UnitOfWork.SaveChangesAsync(cancellation);

            return Result<Guid>.Success(character.Id);
        }
        catch (InvalidGameOperationException ex)
        {
            return Result<Guid>.Invalid(new ValidationError(ex.Message));
        }
        catch (ArgumentException ex)
        {
            return Result<Guid>.Invalid(new ValidationError(ex.Message));
        }
    }
}
