using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using Bunker.Game.API.Models.GameSessions;
using Bunker.Game.Application.Commands.GameSessions.CreateGameSession;
using Bunker.Game.Application.Queries.Characters.Models;
using Bunker.Game.Application.Queries.GameSessions;
using Bunker.Game.Domain.AggregateModels.GameSessions;
using Bunker.Game.Tests.Fixtures;
using Microsoft.AspNetCore.Mvc;

namespace Bunker.Game.Tests.FunctionalTests.GameSessions;

[Collection("BunkerGameApi")]
public class CreateGameSessionTests
{
    private readonly HttpClient _client;
    private readonly BunkerGameApiFixture _fixture;
    private readonly JsonSerializerOptions _jsonOptions;

    public CreateGameSessionTests(BunkerGameApiFixture fixture)
    {
        _client = fixture.CreateClient();
        _fixture = fixture;

        _jsonOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        _jsonOptions.Converters.Add(new JsonStringEnumConverter());
    }

    [Fact]
    public async Task CreateGameSession_WithValidRequest_ReturnsSuccessAndSessionId()
    {
        // Arrange
        var request = new CreateGameSessionRequest
        {
            Name = "Тестовая игра",
            PlayerId = "test-player-id",
            PlayerName = "Тестовый игрок",
            CharactersCount = 5,
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/game-sessions", request, _jsonOptions);

        // Assert
        Assert.True(response.IsSuccessStatusCode);
        var result = await response.Content.ReadFromJsonAsync<GameSessionCreationResult>(_jsonOptions);

        Assert.NotNull(result);
        Assert.NotEqual(Guid.Empty, result.Id);
        Assert.NotEqual(Guid.Empty, result.OccupiedCharacterId);

        var gameSessionId = result.Id;
        var charactersResponse = await _client.GetAsync($"/api/game-sessions/{gameSessionId}/characters");

        Assert.True(charactersResponse.IsSuccessStatusCode);
        var characters = await charactersResponse.Content.ReadFromJsonAsync<IEnumerable<CharacterDto>>(_jsonOptions);

        Assert.NotNull(characters);
        Assert.Equal(request.CharactersCount, characters.Count());
    }

    [Fact]
    public async Task CreateGameSession_WithTooFewCharacters_ReturnsValidationError()
    {
        // Arrange
        var request = new CreateGameSessionRequest
        {
            Name = "Тестовая игра",
            PlayerId = "test-player-id",
            PlayerName = "Тестовый игрок",
            CharactersCount = GameSession.MinCharactersInGame - 1,
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/game-sessions", request, _jsonOptions);

        // Assert
        Assert.False(response.IsSuccessStatusCode);
        var validationErrors = await response.Content.ReadFromJsonAsync<ValidationProblemDetails>(_jsonOptions);

        Assert.NotNull(validationErrors);
        Assert.NotEmpty(validationErrors.Errors);
    }

    [Fact]
    public async Task CreateGameSession_WithTooManyCharacters_ReturnsValidationError()
    {
        // Arrange
        var request = new CreateGameSessionRequest
        {
            Name = "Тестовая игра",
            PlayerId = "test-player-id",
            PlayerName = "Тестовый игрок",
            CharactersCount = GameSession.MaxCharactersInGame + 1,
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/game-sessions", request, _jsonOptions);

        // Assert
        Assert.False(response.IsSuccessStatusCode);
        var validationErrors = await response.Content.ReadFromJsonAsync<ValidationProblemDetails>(_jsonOptions);

        Assert.NotNull(validationErrors);
        Assert.NotEmpty(validationErrors.Errors);
    }

    [Fact]
    public async Task CreateGameSession_WithEmptyName_ReturnsValidationError()
    {
        // Arrange
        var request = new CreateGameSessionRequest
        {
            Name = string.Empty,
            PlayerId = "test-player-id",
            PlayerName = "Тестовый игрок",
            CharactersCount = GameSession.MinCharactersInGame,
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/game-sessions", request, _jsonOptions);

        // Assert
        Assert.False(response.IsSuccessStatusCode);
        var validationErrors = await response.Content.ReadFromJsonAsync<ValidationProblemDetails>(_jsonOptions);

        Assert.NotNull(validationErrors);
        Assert.NotEmpty(validationErrors.Errors);
    }

    [Fact]
    public async Task CreateGameSession_CreatorCharacterIsOccupied()
    {
        // Arrange
        var request = new CreateGameSessionRequest
        {
            Name = "Тестовая игра",
            PlayerId = "test-player-id",
            PlayerName = "Тестовый игрок",
            CharactersCount = GameSession.MinCharactersInGame,
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/game-sessions", request, _jsonOptions);
        var result = await response.Content.ReadFromJsonAsync<GameSessionCreationResult>(_jsonOptions);

        Assert.NotNull(result);

        var gameSessionId = result.Id;
        var occupiedCharacterId = result.OccupiedCharacterId;

        var sessionResponse = await _client.GetAsync($"/api/game-sessions/{gameSessionId}");

        // Assert
        Assert.True(sessionResponse.IsSuccessStatusCode);
        var gameSession = await sessionResponse.Content.ReadFromJsonAsync<GameSessionDto>(_jsonOptions);

        Assert.NotNull(gameSession);

        var occupiedCharacter = gameSession.Characters.FirstOrDefault(c => c.IsGameCreator);
        Assert.NotNull(occupiedCharacter);
        Assert.True(occupiedCharacter.IsOccupiedByPlayer);
        Assert.Equal("test-player-id", occupiedCharacter.PlayerId);
        Assert.Equal("Тестовый игрок", occupiedCharacter.PlayerName);
        Assert.Equal(request.CharactersCount, gameSession.Characters.Count);
    }
}
