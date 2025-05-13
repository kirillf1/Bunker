using Bunker.Game.Domain.AggregateModels.GameSessions.Events;

namespace Bunker.Game.Domain.AggregateModels.GameSessions;

public class GameSession : Entity<Guid>, IAggregateRoot
{
    public const byte MinSeatsCount = 3;

    public const byte MaxSeatsCount = 4;

    public const byte MaxCharactersInGame = 12;

    public const byte MinCharactersInGame = 5;

    private readonly List<Character> _characters;

    public IReadOnlyCollection<Character> Characters => _characters;

    public GameState GameState { get; private set; }

    public string Name { get; private set; }

    public int FreeSeatsCount { get; private set; }

    public string? GameResultDescription { get; private set; }

#pragma warning disable CS8618
    private GameSession(Guid id)
#pragma warning restore CS8618
        : base(id) { }

    private GameSession(Guid id, string name, IEnumerable<Character> characters, Player creator)
        : base(id)
    {
        ValidateName(name);
        Name = name;

        GameState = GameState.Preparing;

        var charactersCount = characters.Count();
        if (charactersCount > MaxCharactersInGame || charactersCount < MinCharactersInGame)
        {
            throw new ArgumentException(
                $"Characters must be more than {MinCharactersInGame - 1} and less {MaxCharactersInGame + 1}"
            );
        }

        _characters = new List<Character>(characters);
        var gameCreator = characters.First();
        gameCreator.OccupyCharacter(creator, true);

        FreeSeatsCount = CalculateFreeSeats(characters);

        AddDomainEvent(new GameSessionCreatedDomainEvent(id));
    }

    public static GameSession CreateGameSession(Guid id, string name, IEnumerable<Character> characters, Player creator)
    {
        if (!characters.Any())
        {
            throw new ArgumentException($"Characters more than {MinCharactersInGame - 1}");
        }

        return new GameSession(id, name, characters, creator);
    }

    public Character OccupyCharacter(Player player)
    {
        if (GameState != GameState.Preparing)
        {
            throw new InvalidGameOperationException($"Game state should be {GameState.Preparing}");
        }

        var occupyCharacterCandidate =
            _characters.FirstOrDefault(x => !x.IsOccupiedByPlayer)
            ?? throw new InvalidGameOperationException($"All characters is occupied");

        occupyCharacterCandidate.OccupyCharacter(player);

        if (_characters.All(x => x.IsOccupiedByPlayer))
        {
            GameState = GameState.Playing;

            AddDomainEvent(new GameSessionStartedDomainEvent(Id));
        }

        return occupyCharacterCandidate;
    }

    public void KickCharacter(Guid characterId)
    {
        if (GameState != GameState.Playing)
        {
            throw new InvalidGameOperationException($"Game state is not {GameState.Playing}");
        }

        var characterForKick =
            _characters.FirstOrDefault(x => x.Id == characterId)
            ?? throw new ArgumentException("Unknown character for kick");

        characterForKick.MarkKicked();

        AddDomainEvent(new CharacterKickedDomainEvent(Id, characterForKick.Id, characterForKick.Player!.Id));

        if (_characters.Count(x => !x.IsKicked) <= FreeSeatsCount)
        {
            GameState = GameState.WaitingForGameResult;

            AddDomainEvent(new EndGameResultRequestedDomainEvent(Id, _characters.Where(x => !x.IsKicked)));
        }
    }

    public void SetGameResult(string gameResultDescription)
    {
        if (GameState != GameState.WaitingForGameResult)
        {
            throw new InvalidGameOperationException($"Game state is not {GameState.WaitingForGameResult}");
        }

        GameResultDescription = gameResultDescription;

        GameState = GameState.Completed;

        AddDomainEvent(new GameSessionCompletedDomainEvent(Id, CompleteReason.NormalCompletion));
    }

    public void TerminateGame()
    {
        if (GameState == GameState.Terminated || GameState == GameState.Completed)
        {
            throw new InvalidGameOperationException("Game session has already been finished");
        }

        GameState = GameState.Terminated;

        AddDomainEvent(new GameSessionCompletedDomainEvent(Id, CompleteReason.ForcedTermination));
    }

    public Character GetGameCreator()
    {
        return Characters.First(x => x.IsGameCreator);
    }

    private static void ValidateName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Name must be not empty");
        }
    }

    private static byte CalculateFreeSeats(IEnumerable<Character> characters)
    {
        if (characters.Count() > 9)
        {
            return MaxSeatsCount;
        }

        return MinSeatsCount;
    }
}
