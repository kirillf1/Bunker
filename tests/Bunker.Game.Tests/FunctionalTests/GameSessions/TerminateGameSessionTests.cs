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
public class TerminateGameSessionTests
{
    private readonly HttpClient _client;
    private readonly BunkerGameApiFixture _fixture;
    private readonly JsonSerializerOptions _jsonOptions;

    public TerminateGameSessionTests(BunkerGameApiFixture fixture)
    {
        _client = fixture.CreateClient();
        _fixture = fixture;

        _jsonOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        _jsonOptions.Converters.Add(new JsonStringEnumConverter());
    }

    [Fact]
    public async Task TerminateGameSession_WithValidRequest_ReturnsSuccess()
    {
        // Arrange
        var creatorId = Guid.NewGuid().ToString();
        var creatorName = Guid.NewGuid().ToString();
        var gameName = Guid.NewGuid().ToString();

        var createRequest = new CreateGameSessionRequest
        {
            Name = gameName,
            PlayerId = creatorId,
            PlayerName = creatorName,
            CharactersCount = GameSession.MinCharactersInGame,
        };

        var createResponse = await _client.PostAsJsonAsync("/api/game-sessions", createRequest, _jsonOptions);
        var createResult = await createResponse.Content.ReadFromJsonAsync<GameSessionCreationResult>(_jsonOptions);
        Assert.NotNull(createResult);

        // Act
        var terminateResponse = await _client.PostAsync($"/api/game-sessions/{createResult.Id}/terminate", null);

        // Assert
        Assert.True(terminateResponse.IsSuccessStatusCode);

        var gameSessionResponse = await _client.GetAsync($"/api/game-sessions/{createResult.Id}");
        Assert.True(gameSessionResponse.IsSuccessStatusCode);
        var gameSession = await gameSessionResponse.Content.ReadFromJsonAsync<GameSessionDto>(_jsonOptions);

        Assert.NotNull(gameSession);
        Assert.Equal(GameState.Terminated, gameSession.GameState);
    }

    [Fact]
    public async Task TerminateGameSession_WithNonExistentGameSession_ReturnsNotFound()
    {
        // Arrange
        var nonExistentGameSessionId = Guid.NewGuid();

        // Act
        var response = await _client.PostAsync($"/api/game-sessions/{nonExistentGameSessionId}/terminate", null);

        // Assert
        Assert.Equal(System.Net.HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task TerminateGameSession_AlreadyTerminatedGame_ReturnsBadRequest()
    {
        // Arrange
        var creatorId = Guid.NewGuid().ToString();
        var creatorName = Guid.NewGuid().ToString();
        var gameName = Guid.NewGuid().ToString();

        var createRequest = new CreateGameSessionRequest
        {
            Name = gameName,
            PlayerId = creatorId,
            PlayerName = creatorName,
            CharactersCount = GameSession.MinCharactersInGame,
        };

        var createResponse = await _client.PostAsJsonAsync("/api/game-sessions", createRequest, _jsonOptions);
        var createResult = await createResponse.Content.ReadFromJsonAsync<GameSessionCreationResult>(_jsonOptions);
        Assert.NotNull(createResult);

        var firstTerminateResponse = await _client.PostAsync($"/api/game-sessions/{createResult.Id}/terminate", null);
        Assert.True(firstTerminateResponse.IsSuccessStatusCode);

        // Act
        var secondTerminateResponse = await _client.PostAsync($"/api/game-sessions/{createResult.Id}/terminate", null);

        // Assert
        Assert.False(secondTerminateResponse.IsSuccessStatusCode);
        Assert.Equal(System.Net.HttpStatusCode.BadRequest, secondTerminateResponse.StatusCode);
    }

    [Fact]
    public async Task TerminateGameSession_WithPlayingGame_ReturnsSuccess()
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

        gameSessionResponse = await _client.GetAsync($"/api/game-sessions/{createResult.Id}");
        gameSession = await gameSessionResponse.Content.ReadFromJsonAsync<GameSessionDto>(_jsonOptions);
        Assert.Equal(GameState.Playing, gameSession.GameState);

        // Act
        var terminateResponse = await _client.PostAsync($"/api/game-sessions/{createResult.Id}/terminate", null);

        // Assert
        Assert.True(terminateResponse.IsSuccessStatusCode);

        gameSessionResponse = await _client.GetAsync($"/api/game-sessions/{createResult.Id}");
        gameSession = await gameSessionResponse.Content.ReadFromJsonAsync<GameSessionDto>(_jsonOptions);

        Assert.NotNull(gameSession);
        Assert.Equal(GameState.Terminated, gameSession.GameState);
    }
}
