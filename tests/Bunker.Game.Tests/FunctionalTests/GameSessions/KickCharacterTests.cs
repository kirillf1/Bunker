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
public class KickCharacterTests
{
    private readonly HttpClient _client;
    private readonly BunkerGameApiFixture _fixture;
    private readonly JsonSerializerOptions _jsonOptions;

    public KickCharacterTests(BunkerGameApiFixture fixture)
    {
        _client = fixture.CreateClient();
        _fixture = fixture;

        _jsonOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        _jsonOptions.Converters.Add(new JsonStringEnumConverter());
    }

    [Fact]
    public async Task KickCharacter_WithValidRequest_ReturnsSuccess()
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

        var freeCharactersCount = gameSession.Characters.Count(c => !c.IsOccupiedByPlayer);
        for (int i = 0; i < freeCharactersCount - 1; i++)
        {
            var playerId = Guid.NewGuid().ToString();
            var playerName = Guid.NewGuid().ToString();

            var occupyRequest = new OccupyCharacterRequest { PlayerId = playerId, PlayerName = playerName };

            await _client.PostAsJsonAsync(
                $"/api/game-sessions/{createResult.Id}/occupy-character",
                occupyRequest,
                _jsonOptions
            );
        }

        var playerToKickId = Guid.NewGuid().ToString();
        var playerToKickName = Guid.NewGuid().ToString();

        var lastOccupyRequest = new OccupyCharacterRequest { PlayerId = playerToKickId, PlayerName = playerToKickName };

        var lastOccupyResponse = await _client.PostAsJsonAsync(
            $"/api/game-sessions/{createResult.Id}/occupy-character",
            lastOccupyRequest,
            _jsonOptions
        );

        Assert.True(lastOccupyResponse.IsSuccessStatusCode);
        var characterIdToKick = await lastOccupyResponse.Content.ReadFromJsonAsync<Guid>(_jsonOptions);
        Assert.NotEqual(Guid.Empty, characterIdToKick);

        gameSessionResponse = await _client.GetAsync($"/api/game-sessions/{createResult.Id}");
        gameSession = await gameSessionResponse.Content.ReadFromJsonAsync<GameSessionDto>(_jsonOptions);
        Assert.NotNull(gameSession);

        Assert.Equal(GameState.Playing, gameSession.GameState);
        Assert.All(gameSession.Characters, c => Assert.True(c.IsOccupiedByPlayer));

        // Act
        var kickResponse = await _client.PostAsync(
            $"/api/game-sessions/{createResult.Id}/characters/{characterIdToKick}/kick",
            null
        );

        // Assert
        Assert.True(kickResponse.IsSuccessStatusCode);

        gameSessionResponse = await _client.GetAsync($"/api/game-sessions/{createResult.Id}");
        gameSession = await gameSessionResponse.Content.ReadFromJsonAsync<GameSessionDto>(_jsonOptions);

        Assert.NotNull(gameSession);
        var kickedCharacter = gameSession.Characters.FirstOrDefault(c => c.Id == characterIdToKick);
        Assert.NotNull(kickedCharacter);
        Assert.True(kickedCharacter.IsKicked);
    }

    [Fact]
    public async Task KickCharacter_WithNonExistentGameSession_ReturnsNotFound()
    {
        // Arrange
        var nonExistentGameSessionId = Guid.NewGuid();
        var characterId = Guid.NewGuid();

        // Act
        var response = await _client.PostAsync(
            $"/api/game-sessions/{nonExistentGameSessionId}/characters/{characterId}/kick",
            null
        );

        // Assert
        Assert.Equal(System.Net.HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task KickCharacter_WithNonExistentCharacter_ReturnsBadRequest()
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

        var freeCharactersCount = gameSession.Characters.Count(c => !c.IsOccupiedByPlayer);
        for (int i = 0; i < freeCharactersCount; i++)
        {
            var playerId = Guid.NewGuid().ToString();
            var playerName = Guid.NewGuid().ToString();

            var occupyRequest = new OccupyCharacterRequest { PlayerId = playerId, PlayerName = playerName };

            await _client.PostAsJsonAsync(
                $"/api/game-sessions/{createResult.Id}/occupy-character",
                occupyRequest,
                _jsonOptions
            );
        }

        var nonExistentCharacterId = Guid.NewGuid();

        // Act
        var response = await _client.PostAsync(
            $"/api/game-sessions/{createResult.Id}/characters/{nonExistentCharacterId}/kick",
            null
        );

        // Assert
        Assert.Equal(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task KickCharacter_WithGameNotStarted_ReturnsBadRequest()
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

        var creatorCharacter = gameSession.Characters.FirstOrDefault(c => c.IsGameCreator);
        Assert.NotNull(creatorCharacter);

        // Act
        var response = await _client.PostAsync(
            $"/api/game-sessions/{createResult.Id}/characters/{creatorCharacter.Id}/kick",
            null
        );

        // Assert
        Assert.False(response.IsSuccessStatusCode);
        Assert.Equal(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
    }
}
