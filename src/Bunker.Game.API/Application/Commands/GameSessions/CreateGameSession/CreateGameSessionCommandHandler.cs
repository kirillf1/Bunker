using Ardalis.Result;
using Bunker.Application.Shared.CQRS;
using Bunker.Domain.Shared.Exceptions;
using Bunker.Game.Domain.AggregateModels.Characters;
using Bunker.Game.Domain.AggregateModels.GameSessions;

namespace Bunker.Game.API.Application.Commands.GameSessions.CreateGameSession;

public class CreateGameSessionCommandHandler
    : ICommandHandler<CreateGameSessionCommand, Result<GameSessionCreationResult>>
{
    private readonly IGameSessionRepository _gameSessionRepository;
    private readonly ICharacterFactory _characterFactory;
    private readonly ICharacterRepository _characterRepository;

    public CreateGameSessionCommandHandler(
        IGameSessionRepository gameSessionRepository,
        ICharacterFactory characterFactory,
        ICharacterRepository characterRepository
    )
    {
        _gameSessionRepository = gameSessionRepository;
        _characterFactory = characterFactory;
        _characterRepository = characterRepository;
    }

    public async Task<Result<GameSessionCreationResult>> Handle(
        CreateGameSessionCommand command,
        CancellationToken cancellation
    )
    {
        try
        {
            var gameSessionId = command.GameSessionId;

            var gameSessionCharacters = await CreateCharacters(command, gameSessionId);

            var creator = new Player(command.PlayerId, command.PlayerName);

            var gameSession = GameSession.CreateGameSession(
                gameSessionId,
                command.Name,
                gameSessionCharacters,
                creator
            );

            await _gameSessionRepository.Add(gameSession);

            await _gameSessionRepository.UnitOfWork.SaveChangesAsync(cancellation);

            return Result.Success(new GameSessionCreationResult(gameSession.Id, gameSession.GetGameCreator().Id));
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

    private async Task<IEnumerable<Domain.AggregateModels.GameSessions.Character>> CreateCharacters(
        CreateGameSessionCommand command,
        Guid gameSessionId
    )
    {
        var characters = await _characterFactory.CreateCharacters(gameSessionId, command.CharactersCount);
        var gameSessionCharacters = new List<Domain.AggregateModels.GameSessions.Character>();

        foreach (var character in characters)
        {
            gameSessionCharacters.Add(new Domain.AggregateModels.GameSessions.Character(character.Id));
        }

        await _characterRepository.UnitOfWork.SaveChangesAsync();

        return gameSessionCharacters;
    }
}
