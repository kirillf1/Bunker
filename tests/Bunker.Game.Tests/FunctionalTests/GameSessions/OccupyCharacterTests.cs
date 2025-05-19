using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using Bunker.Game.API.Models.GameSessions;
using Bunker.Game.Application.Commands.GameSessions.CreateGameSession;
using Bunker.Game.Application.Queries.GameSessions;
using Bunker.Game.Domain.AggregateModels.GameSessions;
using Bunker.Game.Tests.Fixtures;
using Microsoft.AspNetCore.Mvc;

namespace Bunker.Game.Tests.FunctionalTests.GameSessions;

[Collection("BunkerGameApi")]
public class OccupyCharacterTests
{
    private readonly HttpClient _client;
    private readonly BunkerGameApiFixture _fixture;
    private readonly JsonSerializerOptions _jsonOptions;

    public OccupyCharacterTests(BunkerGameApiFixture fixture)
    {
        _client = fixture.CreateClient();
        _fixture = fixture;

        _jsonOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        _jsonOptions.Converters.Add(new JsonStringEnumConverter());
    }

    [Fact]
    public async Task OccupyCharacter_WithValidRequest_ReturnsSuccessAndCharacterId()
    {
        // Arrange
        var creatorId = Guid.NewGuid().ToString();
        var creatorName = Guid.NewGuid().ToString();
        var gameName = Guid.NewGuid().ToString();
        var newPlayerId = Guid.NewGuid().ToString();
        var newPlayerName = Guid.NewGuid().ToString();

        var createRequest = new CreateGameSessionRequest
        {
            Name = gameName,
            PlayerId = creatorId,
            PlayerName = creatorName,
            CharactersCount = 5,
        };

        var createResponse = await _client.PostAsJsonAsync("/api/game-sessions", createRequest, _jsonOptions);
        var createResult = await createResponse.Content.ReadFromJsonAsync<GameSessionCreationResult>(_jsonOptions);
        Assert.NotNull(createResult);

        var occupyRequest = new OccupyCharacterRequest { PlayerId = newPlayerId, PlayerName = newPlayerName };

        // Act
        var response = await _client.PostAsJsonAsync(
            $"/api/game-sessions/{createResult.Id}/occupy-character",
            occupyRequest,
            _jsonOptions
        );

        // Assert
        Assert.True(response.IsSuccessStatusCode);
        var result = await response.Content.ReadFromJsonAsync<Guid>(_jsonOptions);
        Assert.NotEqual(Guid.Empty, result);

        var gameSessionResponse = await _client.GetAsync($"/api/game-sessions/{createResult.Id}");
        Assert.True(gameSessionResponse.IsSuccessStatusCode);
        var gameSession = await gameSessionResponse.Content.ReadFromJsonAsync<GameSessionDto>(_jsonOptions);

        Assert.NotNull(gameSession);
        var occupiedCharacter = gameSession.Characters.FirstOrDefault(c => c.Id == result);
        Assert.NotNull(occupiedCharacter);
        Assert.True(occupiedCharacter.IsOccupiedByPlayer);
        Assert.Equal(newPlayerId, occupiedCharacter.PlayerId);
        Assert.Equal(newPlayerName, occupiedCharacter.PlayerName);
    }

    [Fact]
    public async Task OccupyCharacter_WithNonExistentGameSession_ReturnsNotFound()
    {
        // Arrange
        var playerId = Guid.NewGuid().ToString();
        var playerName = Guid.NewGuid().ToString();

        var request = new OccupyCharacterRequest { PlayerId = playerId, PlayerName = playerName };

        // Act
        var response = await _client.PostAsJsonAsync(
            $"/api/game-sessions/{Guid.NewGuid()}/occupy-character",
            request,
            _jsonOptions
        );

        // Assert
        Assert.Equal(System.Net.HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task OccupyCharacter_WithEmptyPlayerName_ReturnsValidationError()
    {
        // Arrange
        var creatorId = Guid.NewGuid().ToString();
        var creatorName = Guid.NewGuid().ToString();
        var gameName = Guid.NewGuid().ToString();
        var playerId = Guid.NewGuid().ToString();

        var createRequest = new CreateGameSessionRequest
        {
            Name = gameName,
            PlayerId = creatorId,
            PlayerName = creatorName,
            CharactersCount = 5,
        };

        var createResponse = await _client.PostAsJsonAsync("/api/game-sessions", createRequest, _jsonOptions);
        var createResult = await createResponse.Content.ReadFromJsonAsync<GameSessionCreationResult>(_jsonOptions);
        Assert.NotNull(createResult);

        var occupyRequest = new OccupyCharacterRequest { PlayerId = playerId, PlayerName = string.Empty };

        // Act
        var response = await _client.PostAsJsonAsync(
            $"/api/game-sessions/{createResult.Id}/occupy-character",
            occupyRequest,
            _jsonOptions
        );

        // Assert
        Assert.False(response.IsSuccessStatusCode);
    }

    [Fact]
    public async Task OccupyCharacter_WithEmptyPlayerId_ReturnsValidationError()
    {
        // Arrange
        var creatorId = Guid.NewGuid().ToString();
        var creatorName = Guid.NewGuid().ToString();
        var gameName = Guid.NewGuid().ToString();
        var playerName = Guid.NewGuid().ToString();

        var createRequest = new CreateGameSessionRequest
        {
            Name = gameName,
            PlayerId = creatorId,
            PlayerName = creatorName,
            CharactersCount = 5,
        };

        var createResponse = await _client.PostAsJsonAsync("/api/game-sessions", createRequest, _jsonOptions);
        var createResult = await createResponse.Content.ReadFromJsonAsync<GameSessionCreationResult>(_jsonOptions);
        Assert.NotNull(createResult);

        var occupyRequest = new OccupyCharacterRequest { PlayerId = string.Empty, PlayerName = playerName };

        // Act
        var response = await _client.PostAsJsonAsync(
            $"/api/game-sessions/{createResult.Id}/occupy-character",
            occupyRequest,
            _jsonOptions
        );

        // Assert
        Assert.False(response.IsSuccessStatusCode);
    }
}
