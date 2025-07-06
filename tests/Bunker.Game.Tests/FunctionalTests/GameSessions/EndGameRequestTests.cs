using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using Bunker.Game.API.Models.GameSessions;
using Bunker.Game.Application.Commands.GameSessions.CreateGameSession;
using Bunker.Game.Application.Queries.GameSessions;
using Bunker.Game.Domain.AggregateModels.GameSessions;
using Bunker.Game.Tests.Fixtures;

namespace Bunker.Game.Tests.FunctionalTests.GameSessions;

[Collection("BunkerGameApi")]
public class EndGameRequestTests
{
    private readonly HttpClient _client;
    private readonly BunkerGameApiFixture _fixture;
    private readonly JsonSerializerOptions _jsonOptions;

    public EndGameRequestTests(BunkerGameApiFixture fixture)
    {
        _client = fixture.CreateClient();
        _fixture = fixture;

        _jsonOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        _jsonOptions.Converters.Add(new JsonStringEnumConverter());
    }

    [Fact]
    public async Task KickCharacter_WhenNotKickedCharactersEqualsToFreeSeats_TriggersEndGameRequest()
    {
        // Arrange
        const int charactersCount = GameSession.MinCharactersInGame;
        var creatorId = Guid.NewGuid().ToString();
        var creatorName = Guid.NewGuid().ToString();
        var gameName = Guid.NewGuid().ToString();

        var createRequest = new CreateGameSessionRequest
        {
            Name = gameName,
            PlayerId = creatorId,
            PlayerName = creatorName,
            CharactersCount = charactersCount,
        };

        var createResponse = await _client.PostAsJsonAsync("/api/game-sessions", createRequest, _jsonOptions);
        var createResult = await createResponse.Content.ReadFromJsonAsync<GameSessionCreationResult>(_jsonOptions);
        Assert.NotNull(createResult);

        var gameSessionResponse = await _client.GetAsync($"/api/game-sessions/{createResult.Id}");
        var gameSession = await gameSessionResponse.Content.ReadFromJsonAsync<GameSessionDto>(_jsonOptions);
        Assert.NotNull(gameSession);

        // Занимаем всех персонажей игроками
        var playersToKick = new List<(string PlayerId, string PlayerName, Guid CharacterId)>();
        var freeCharactersCount = gameSession.Characters.Count(c => !c.IsOccupiedByPlayer);

        for (int i = 0; i < freeCharactersCount; i++)
        {
            var playerId = Guid.NewGuid().ToString();
            var playerName = Guid.NewGuid().ToString();

            var occupyRequest = new OccupyCharacterRequest { PlayerId = playerId, PlayerName = playerName };

            var occupyResponse = await _client.PostAsJsonAsync(
                $"/api/game-sessions/{createResult.Id}/occupy-character",
                occupyRequest,
                _jsonOptions
            );

            Assert.True(occupyResponse.IsSuccessStatusCode);
            var characterId = await occupyResponse.Content.ReadFromJsonAsync<Guid>(_jsonOptions);
            playersToKick.Add((playerId, playerName, characterId));
        }

        // Проверяем, что игра началась
        gameSessionResponse = await _client.GetAsync($"/api/game-sessions/{createResult.Id}");
        gameSession = await gameSessionResponse.Content.ReadFromJsonAsync<GameSessionDto>(_jsonOptions);
        Assert.NotNull(gameSession);
        Assert.Equal(GameState.Playing, gameSession.GameState);
        Assert.All(gameSession.Characters, c => Assert.True(c.IsOccupiedByPlayer));

        // Кикаем первого персонажа (должно остаться 4 не кикнутых)
        var firstKickResponse = await _client.PostAsync(
            $"/api/game-sessions/{createResult.Id}/characters/{playersToKick[0].CharacterId}/kick",
            null
        );

        Assert.True(firstKickResponse.IsSuccessStatusCode);

        // Проверяем, что игра еще не завершена
        gameSessionResponse = await _client.GetAsync($"/api/game-sessions/{createResult.Id}");
        gameSession = await gameSessionResponse.Content.ReadFromJsonAsync<GameSessionDto>(_jsonOptions);
        Assert.NotNull(gameSession);
        Assert.Equal(GameState.Playing, gameSession.GameState);
        Assert.Equal(1, gameSession.Characters.Count(c => c.IsKicked));
        Assert.Equal(4, gameSession.Characters.Count(c => !c.IsKicked));

        // Act: Кикаем второго персонажа (должно остаться 3 не кикнутых = FreeSeatsCount)
        var secondKickResponse = await _client.PostAsync(
            $"/api/game-sessions/{createResult.Id}/characters/{playersToKick[1].CharacterId}/kick",
            null
        );

        // Assert
        Assert.True(secondKickResponse.IsSuccessStatusCode);

        // Проверяем, что игра перешла в состояние WaitingForGameResult
        gameSessionResponse = await _client.GetAsync($"/api/game-sessions/{createResult.Id}");
        gameSession = await gameSessionResponse.Content.ReadFromJsonAsync<GameSessionDto>(_jsonOptions);
        Assert.NotNull(gameSession);
        Assert.Equal(GameState.WaitingForGameResult, gameSession.GameState);
        Assert.Equal(2, gameSession.Characters.Count(c => c.IsKicked));
        Assert.Equal(3, gameSession.Characters.Count(c => !c.IsKicked)); // 3 = FreeSeatsCount для 5 персонажей
    }

    [Fact]
    public async Task KickCharacter_WithMaxCharactersCount_TriggersEndGameRequestWhenReachesFreeSeatsLimit()
    {
        // Arrange
        const int charactersCount = GameSession.MaxCharactersInGame; // 12 персонажей
        var creatorId = Guid.NewGuid().ToString();
        var creatorName = Guid.NewGuid().ToString();
        var gameName = Guid.NewGuid().ToString();

        var createRequest = new CreateGameSessionRequest
        {
            Name = gameName,
            PlayerId = creatorId,
            PlayerName = creatorName,
            CharactersCount = charactersCount,
        };

        var createResponse = await _client.PostAsJsonAsync("/api/game-sessions", createRequest, _jsonOptions);
        var createResult = await createResponse.Content.ReadFromJsonAsync<GameSessionCreationResult>(_jsonOptions);
        Assert.NotNull(createResult);

        var gameSessionResponse = await _client.GetAsync($"/api/game-sessions/{createResult.Id}");
        var gameSession = await gameSessionResponse.Content.ReadFromJsonAsync<GameSessionDto>(_jsonOptions);
        Assert.NotNull(gameSession);

        // Занимаем всех персонажей игроками
        var playersToKick = new List<(string PlayerId, string PlayerName, Guid CharacterId)>();
        var freeCharactersCount = gameSession.Characters.Count(c => !c.IsOccupiedByPlayer);

        for (int i = 0; i < freeCharactersCount; i++)
        {
            var playerId = Guid.NewGuid().ToString();
            var playerName = Guid.NewGuid().ToString();

            var occupyRequest = new OccupyCharacterRequest { PlayerId = playerId, PlayerName = playerName };

            var occupyResponse = await _client.PostAsJsonAsync(
                $"/api/game-sessions/{createResult.Id}/occupy-character",
                occupyRequest,
                _jsonOptions
            );

            Assert.True(occupyResponse.IsSuccessStatusCode);
            var characterId = await occupyResponse.Content.ReadFromJsonAsync<Guid>(_jsonOptions);
            playersToKick.Add((playerId, playerName, characterId));
        }

        // Проверяем, что игра началась
        gameSessionResponse = await _client.GetAsync($"/api/game-sessions/{createResult.Id}");
        gameSession = await gameSessionResponse.Content.ReadFromJsonAsync<GameSessionDto>(_jsonOptions);
        Assert.NotNull(gameSession);
        Assert.Equal(GameState.Playing, gameSession.GameState);

        // Кикаем персонажей до тех пор, пока не останется 5 не кикнутых (12 - 7 = 5)
        for (int i = 0; i < 7; i++)
        {
            var kickResponse = await _client.PostAsync(
                $"/api/game-sessions/{createResult.Id}/characters/{playersToKick[i].CharacterId}/kick",
                null
            );
            Assert.True(kickResponse.IsSuccessStatusCode);
        }

        // Проверяем, что игра еще в состоянии Playing
        gameSessionResponse = await _client.GetAsync($"/api/game-sessions/{createResult.Id}");
        gameSession = await gameSessionResponse.Content.ReadFromJsonAsync<GameSessionDto>(_jsonOptions);
        Assert.NotNull(gameSession);
        Assert.Equal(GameState.Playing, gameSession.GameState);
        Assert.Equal(7, gameSession.Characters.Count(c => c.IsKicked));
        Assert.Equal(5, gameSession.Characters.Count(c => !c.IsKicked));

        // Act: Кикаем еще одного персонажа (должно остаться 4 не кикнутых = FreeSeatsCount для 12 персонажей)
        var finalKickResponse = await _client.PostAsync(
            $"/api/game-sessions/{createResult.Id}/characters/{playersToKick[7].CharacterId}/kick",
            null
        );

        // Assert
        Assert.True(finalKickResponse.IsSuccessStatusCode);

        // Проверяем, что игра перешла в состояние WaitingForGameResult
        gameSessionResponse = await _client.GetAsync($"/api/game-sessions/{createResult.Id}");
        gameSession = await gameSessionResponse.Content.ReadFromJsonAsync<GameSessionDto>(_jsonOptions);
        Assert.NotNull(gameSession);
        Assert.Equal(GameState.WaitingForGameResult, gameSession.GameState);
        Assert.Equal(8, gameSession.Characters.Count(c => c.IsKicked));
        Assert.Equal(4, gameSession.Characters.Count(c => !c.IsKicked)); // 4 = FreeSeatsCount для 12 персонажей
    }

    [Fact]
    public async Task KickCharacter_BeforeReachingFreeSeatsLimit_DoesNotTriggerEndGameRequest()
    {
        // Arrange
        const int charactersCount = GameSession.MinCharactersInGame + 2; // 7 персонажей
        var creatorId = Guid.NewGuid().ToString();
        var creatorName = Guid.NewGuid().ToString();
        var gameName = Guid.NewGuid().ToString();

        var createRequest = new CreateGameSessionRequest
        {
            Name = gameName,
            PlayerId = creatorId,
            PlayerName = creatorName,
            CharactersCount = charactersCount,
        };

        var createResponse = await _client.PostAsJsonAsync("/api/game-sessions", createRequest, _jsonOptions);
        var createResult = await createResponse.Content.ReadFromJsonAsync<GameSessionCreationResult>(_jsonOptions);
        Assert.NotNull(createResult);

        var gameSessionResponse = await _client.GetAsync($"/api/game-sessions/{createResult.Id}");
        var gameSession = await gameSessionResponse.Content.ReadFromJsonAsync<GameSessionDto>(_jsonOptions);
        Assert.NotNull(gameSession);

        // Занимаем всех персонажей игроками
        var playersToKick = new List<(string PlayerId, string PlayerName, Guid CharacterId)>();
        var freeCharactersCount = gameSession.Characters.Count(c => !c.IsOccupiedByPlayer);

        for (int i = 0; i < freeCharactersCount; i++)
        {
            var playerId = Guid.NewGuid().ToString();
            var playerName = Guid.NewGuid().ToString();

            var occupyRequest = new OccupyCharacterRequest { PlayerId = playerId, PlayerName = playerName };

            var occupyResponse = await _client.PostAsJsonAsync(
                $"/api/game-sessions/{createResult.Id}/occupy-character",
                occupyRequest,
                _jsonOptions
            );

            Assert.True(occupyResponse.IsSuccessStatusCode);
            var characterId = await occupyResponse.Content.ReadFromJsonAsync<Guid>(_jsonOptions);
            playersToKick.Add((playerId, playerName, characterId));
        }

        // Проверяем, что игра началась
        gameSessionResponse = await _client.GetAsync($"/api/game-sessions/{createResult.Id}");
        gameSession = await gameSessionResponse.Content.ReadFromJsonAsync<GameSessionDto>(_jsonOptions);
        Assert.NotNull(gameSession);
        Assert.Equal(GameState.Playing, gameSession.GameState);

        // Act: Кикаем персонажей, но не достигаем лимита (7 -> 5, но FreeSeatsCount = 3)
        for (int i = 0; i < 2; i++)
        {
            var kickResponse = await _client.PostAsync(
                $"/api/game-sessions/{createResult.Id}/characters/{playersToKick[i].CharacterId}/kick",
                null
            );
            Assert.True(kickResponse.IsSuccessStatusCode);
        }

        // Assert: Игра должна оставаться в состоянии Playing
        gameSessionResponse = await _client.GetAsync($"/api/game-sessions/{createResult.Id}");
        gameSession = await gameSessionResponse.Content.ReadFromJsonAsync<GameSessionDto>(_jsonOptions);
        Assert.NotNull(gameSession);
        Assert.Equal(GameState.Playing, gameSession.GameState);
        Assert.Equal(2, gameSession.Characters.Count(c => c.IsKicked));
        Assert.Equal(5, gameSession.Characters.Count(c => !c.IsKicked)); // 5 > 3 (FreeSeatsCount)
    }
}
