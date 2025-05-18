using Ardalis.Result;
using Bunker.Application.Shared.CQRS;
using Bunker.Game.Domain.AggregateModels.Characters;
using Bunker.Game.Domain.AggregateModels.GameSessions;

namespace Bunker.Game.Application.Commands.GameSessions.CreateGameSession;

public class CreateGameSessionCommandHandler
    : ICommandHandler<CreateGameSessionCommand, Result<GameSessionCreationResult>>
{
    private readonly IGameSessionRepository _gameSessionRepository;
    private readonly ICharacterFactory _characterFactory;
    private readonly ICharacterRepository _characterRepository;
    private readonly ILogger<CreateGameSessionCommandHandler> _logger;

    public CreateGameSessionCommandHandler(
        IGameSessionRepository gameSessionRepository,
        ICharacterFactory characterFactory,
        ICharacterRepository characterRepository,
        ILogger<CreateGameSessionCommandHandler> logger
    )
    {
        _gameSessionRepository = gameSessionRepository;
        _characterFactory = characterFactory;
        _characterRepository = characterRepository;
        _logger = logger;
    }

    public async Task<Result<GameSessionCreationResult>> Handle(
        CreateGameSessionCommand command,
        CancellationToken cancellation
    )
    {
        try
        {
            _logger.LogInformation(
                "Creating new game session with name '{GameName}', requested by player {PlayerId}",
                command.Name,
                command.PlayerId
            );

            var gameSessionId = command.GameSessionId;

            var gameSessionCharacters = await CreateCharacters(command, gameSessionId);
            _logger.LogInformation(
                "Created {CharacterCount} characters for game session {GameSessionId}",
                gameSessionCharacters.Count(),
                gameSessionId
            );

            var creator = new Player(command.PlayerId, command.PlayerName);

            var gameSession = GameSession.CreateGameSession(
                gameSessionId,
                command.Name,
                gameSessionCharacters,
                creator
            );

            await _gameSessionRepository.Add(gameSession);

            await _gameSessionRepository.UnitOfWork.SaveChangesAsync(cancellation);

            _logger.LogInformation(
                "Game session {GameSessionId} successfully created by player {PlayerId}",
                gameSession.Id,
                command.PlayerId
            );

            return Result.Success(new GameSessionCreationResult(gameSession.Id, gameSession.GetGameCreator().Id));
        }
        catch (InvalidGameOperationException ex)
        {
            _logger.LogError(ex, "Failed to create game session due to invalid operation: {ErrorMessage}", ex.Message);
            return Result.Invalid(new ValidationError(ex.Message));
        }
        catch (ArgumentException ex)
        {
            _logger.LogError(ex, "Failed to create game session due to invalid argument: {ErrorMessage}", ex.Message);
            return Result.Invalid(new ValidationError(ex.Message));
        }
    }

    private async Task<IEnumerable<Domain.AggregateModels.GameSessions.Character>> CreateCharacters(
        CreateGameSessionCommand command,
        Guid gameSessionId
    )
    {
        _logger.LogInformation(
            "Generating {CharacterCount} characters for game session {GameSessionId}",
            command.CharactersCount,
            gameSessionId
        );

        var characters = await _characterFactory.CreateCharacters(gameSessionId, command.CharactersCount);
        var gameSessionCharacters = new List<Domain.AggregateModels.GameSessions.Character>();

        foreach (var character in characters)
        {
            gameSessionCharacters.Add(new Domain.AggregateModels.GameSessions.Character(character.Id));

            await _characterRepository.Add(character);
        }

        await _characterRepository.UnitOfWork.SaveChangesAsync();

        return gameSessionCharacters;
    }
}
