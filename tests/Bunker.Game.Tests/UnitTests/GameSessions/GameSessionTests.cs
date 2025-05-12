using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bunker.Domain.Shared.Exceptions;
using Bunker.Game.Domain.AggregateModels.GameSessions;
using Bunker.Game.Domain.AggregateModels.GameSessions.Events;

namespace Bunker.Game.Tests.UnitTests.GameSessions
{
    public class GameSessionTests
    {
        private readonly Guid _gameSessionId = Guid.NewGuid();
        private readonly string _name = "Test Game";
        private readonly List<Character> _characters;
        private readonly Player _creator;

        public GameSessionTests()
        {
            _creator = new Player("player1", "Creator");
            _characters = Enumerable
                .Range(1, GameSession.MinCharactersInGame)
                .Select(i => new Character(Guid.NewGuid()))
                .ToList();
        }

        [Fact]
        public void CreateGameSession_ValidParameters_CreatesSession()
        {
            // Arrange
            var characters = _characters;
            var creator = _creator;

            // Act
            var gameSession = GameSession.CreateGameSession(_gameSessionId, _name, characters, creator);

            // Assert
            Assert.Equal(_gameSessionId, gameSession.Id);
            Assert.Equal(_name, gameSession.Name);
            Assert.Equal(GameState.Preparing, gameSession.GameState);
            Assert.Equal(characters.Count, gameSession.Characters.Count);
            Assert.Equal(GameSession.MinSeatsCount, gameSession.FreeSeatsCount);
            Assert.True(gameSession.Characters.First().IsOccupiedByPlayer);
            Assert.Contains(gameSession.DomainEvents, e => e is GameSessionCreatedDomainEvent);
        }

        [Fact]
        public void CreateGameSession_EmptyCharacters_ThrowsArgumentException()
        {
            // Arrange
            var emptyCharacters = new List<Character>();

            // Act & Assert
            Assert.Throws<ArgumentException>(
                () => GameSession.CreateGameSession(_gameSessionId, _name, emptyCharacters, _creator)
            );
        }

        [Fact]
        public void CreateGameSession_TooFewCharacters_ThrowsArgumentException()
        {
            // Arrange
            var tooFewCharacters = Enumerable
                .Range(1, GameSession.MinCharactersInGame - 1)
                .Select(i => new Character(Guid.NewGuid()))
                .ToList();

            // Act & Assert
            Assert.Throws<ArgumentException>(
                () => GameSession.CreateGameSession(_gameSessionId, _name, tooFewCharacters, _creator)
            );
        }

        [Fact]
        public void CreateGameSession_TooManyCharacters_ThrowsArgumentException()
        {
            // Arrange
            var tooManyCharacters = Enumerable
                .Range(1, GameSession.MaxCharactersInGame + 1)
                .Select(i => new Character(Guid.NewGuid()))
                .ToList();

            // Act & Assert
            Assert.Throws<ArgumentException>(
                () => GameSession.CreateGameSession(_gameSessionId, _name, tooManyCharacters, _creator)
            );
        }

        [Fact]
        public void CreateGameSession_EmptyName_ThrowsArgumentException()
        {
            // Arrange
            var characters = _characters;
            var emptyName = "";

            // Act & Assert
            Assert.Throws<ArgumentException>(
                () => GameSession.CreateGameSession(_gameSessionId, emptyName, characters, _creator)
            );
        }

        [Fact]
        public void OccupyCharacter_AvailableCharacter_OccupiesAndUpdatesState()
        {
            // Arrange
            var gameSession = GameSession.CreateGameSession(_gameSessionId, _name, _characters, _creator);
            var player = new Player("player2", "Player2");

            // Act
            var occupiedCharacter = gameSession.OccupyCharacter(player);

            // Assert
            Assert.NotNull(occupiedCharacter.Player);
            Assert.Equal(player, occupiedCharacter.Player);
            Assert.True(occupiedCharacter.IsOccupiedByPlayer);
        }

        [Fact]
        public void OccupyCharacter_AllCharactersOccupied_TransitionsToPlaying()
        {
            // Arrange
            var characters = Enumerable
                .Range(1, GameSession.MinCharactersInGame)
                .Select(i => new Character(Guid.NewGuid()))
                .ToList();
            var gameSession = GameSession.CreateGameSession(_gameSessionId, _name, characters, _creator);

            // Act
            OccupyAllCharacters(gameSession);

            // Assert
            Assert.Equal(GameState.Playing, gameSession.GameState);
            Assert.Contains(gameSession.DomainEvents, e => e is GameSessionStartedDomainEvent);
        }

        [Fact]
        public void OccupyCharacter_NotPreparingState_ThrowsInvalidGameOperationException()
        {
            // Arrange
            var characters = Enumerable
                .Range(1, GameSession.MinCharactersInGame)
                .Select(i => new Character(Guid.NewGuid()))
                .ToList();
            var gameSession = GameSession.CreateGameSession(_gameSessionId, _name, characters, _creator);
            OccupyAllCharacters(gameSession); // Переводим в Playing
            var extraPlayer = new Player("player4", "Player4");

            // Act & Assert
            Assert.Throws<InvalidGameOperationException>(() => gameSession.OccupyCharacter(extraPlayer));
        }

        [Fact]
        public void OccupyCharacter_NoAvailableCharacters_ThrowsInvalidGameOperationException()
        {
            // Arrange
            var characters = Enumerable
                .Range(1, GameSession.MinCharactersInGame)
                .Select(i => new Character(Guid.NewGuid()))
                .ToList();
            var gameSession = GameSession.CreateGameSession(_gameSessionId, _name, characters, _creator);
            OccupyAllCharacters(gameSession);
            var extraPlayer = new Player("player4", "Player4");

            // Act & Assert
            Assert.Throws<InvalidGameOperationException>(() => gameSession.OccupyCharacter(extraPlayer));
        }

        [Fact]
        public void KickCharacter_ValidCharacter_MarksKickedAndRaisesEvent()
        {
            // Arrange
            var characters = Enumerable
                .Range(1, GameSession.MinCharactersInGame)
                .Select(i => new Character(Guid.NewGuid()))
                .ToList();
            var gameSession = GameSession.CreateGameSession(_gameSessionId, _name, characters, _creator);
            OccupyAllCharacters(gameSession); // Переводим в Playing
            var characterToKick = characters.First(c => c.IsOccupiedByPlayer);

            // Act
            gameSession.KickCharacter(characterToKick.Id);

            // Assert
            Assert.True(characterToKick.IsKicked);
            Assert.Contains(gameSession.DomainEvents, e => e is CharacterKickedDomainEvent);
        }

        [Fact]
        public void KickCharacter_EnoughKicked_TransitionsToWaitingForGameResult()
        {
            // Arrange
            var characters = Enumerable
                .Range(1, GameSession.MinCharactersInGame)
                .Select(i => new Character(Guid.NewGuid()))
                .ToList();
            var gameSession = GameSession.CreateGameSession(_gameSessionId, _name, characters, _creator);
            OccupyAllCharacters(gameSession); // Переводим в Playing

            // Act
            KickCharactersToWaitingForGameResult(gameSession, characters);

            // Assert
            Assert.Equal(GameState.WaitingForGameResult, gameSession.GameState);
            Assert.Contains(gameSession.DomainEvents, e => e is EndGameResultRequestedDomainEvent);
        }

        [Fact]
        public void KickCharacter_NotPlayingState_ThrowsInvalidGameOperationException()
        {
            // Arrange
            var gameSession = GameSession.CreateGameSession(_gameSessionId, _name, _characters, _creator);
            var characterToKick = _characters.First();

            // Act & Assert
            Assert.Throws<InvalidGameOperationException>(() => gameSession.KickCharacter(characterToKick.Id));
        }

        [Fact]
        public void KickCharacter_UnknownCharacter_ThrowsArgumentException()
        {
            // Arrange
            var characters = Enumerable
                .Range(1, GameSession.MinCharactersInGame)
                .Select(i => new Character(Guid.NewGuid()))
                .ToList();
            var gameSession = GameSession.CreateGameSession(_gameSessionId, _name, characters, _creator);
            OccupyAllCharacters(gameSession); // Переводим в Playing
            var unknownCharacterId = Guid.NewGuid();

            // Act & Assert
            Assert.Throws<ArgumentException>(() => gameSession.KickCharacter(unknownCharacterId));
        }

        [Fact]
        public void SetGameResult_ValidState_SetsResultAndCompletes()
        {
            // Arrange
            var characters = Enumerable
                .Range(1, GameSession.MinCharactersInGame)
                .Select(i => new Character(Guid.NewGuid()))
                .ToList();
            var gameSession = GameSession.CreateGameSession(_gameSessionId, _name, characters, _creator);
            OccupyAllCharacters(gameSession); // Переводим в Playing
            KickCharactersToWaitingForGameResult(gameSession, characters); // Переводим в WaitingForGameResult
            var resultDescription = "Game ended successfully";

            // Act
            gameSession.SetGameResult(resultDescription);

            // Assert
            Assert.Equal(GameState.Completed, gameSession.GameState);
            Assert.Equal(resultDescription, gameSession.GameResultDescription);
            Assert.Contains(gameSession.DomainEvents, e => e is GameSessionCompletedDomainEvent);
        }

        [Fact]
        public void SetGameResult_NotWaitingForGameResult_ThrowsInvalidGameOperationException()
        {
            // Arrange
            var gameSession = GameSession.CreateGameSession(_gameSessionId, _name, _characters, _creator);
            var resultDescription = "Game ended successfully";

            // Act & Assert
            Assert.Throws<InvalidGameOperationException>(() => gameSession.SetGameResult(resultDescription));
        }

        [Fact]
        public void TerminateGame_ValidState_TerminatesSession()
        {
            // Arrange
            var gameSession = GameSession.CreateGameSession(_gameSessionId, _name, _characters, _creator);

            // Act
            gameSession.TerminateGame();

            // Assert
            Assert.Equal(GameState.Terminated, gameSession.GameState);
            Assert.Contains(gameSession.DomainEvents, e => e is GameSessionCompletedDomainEvent);
        }

        [Fact]
        public void TerminateGame_AlreadyTerminated_ThrowsInvalidGameOperationException()
        {
            // Arrange
            var gameSession = GameSession.CreateGameSession(_gameSessionId, _name, _characters, _creator);
            gameSession.TerminateGame();

            // Act & Assert
            Assert.Throws<InvalidGameOperationException>(() => gameSession.TerminateGame());
        }

        [Fact]
        public void TerminateGame_AlreadyCompleted_ThrowsInvalidGameOperationException()
        {
            // Arrange
            var characters = Enumerable
                .Range(1, GameSession.MinCharactersInGame)
                .Select(i => new Character(Guid.NewGuid()))
                .ToList();
            var gameSession = GameSession.CreateGameSession(_gameSessionId, _name, characters, _creator);
            OccupyAllCharacters(gameSession); // Переводим в Playing
            KickCharactersToWaitingForGameResult(gameSession, characters); // Переводим в WaitingForGameResult
            gameSession.SetGameResult("Game ended"); // Переводим в Completed

            // Act & Assert
            Assert.Throws<InvalidGameOperationException>(() => gameSession.TerminateGame());
        }

        private void OccupyAllCharacters(GameSession gameSession)
        {
            var unoccupiedCount = gameSession.Characters.Count(c => !c.IsOccupiedByPlayer);
            var players = Enumerable
                .Range(1, unoccupiedCount)
                .Select(i => new Player($"player{i + 1}", $"Player{i + 1}"))
                .ToList();
            foreach (var player in players)
            {
                gameSession.OccupyCharacter(player);
            }
        }

        private void KickCharactersToWaitingForGameResult(GameSession gameSession, List<Character> characters)
        {
            var charactersToKick = characters
                .Where(c => c.IsOccupiedByPlayer)
                .Take(GameSession.MinCharactersInGame - gameSession.FreeSeatsCount)
                .ToList();
            foreach (var character in charactersToKick)
            {
                gameSession.KickCharacter(character.Id);
            }
        }
    }
}
