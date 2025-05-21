using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using Bunker.Game.API.Models.GameSessions;
using Bunker.Game.Application.Commands.GameSessions.CreateGameSession;
using Bunker.Game.Application.Queries.Bunkers;
using Bunker.Game.Application.Queries.GameSessions;
using Bunker.Game.Domain.AggregateModels.GameSessions;
using Bunker.Game.Tests.Fixtures;

namespace Bunker.Game.Tests.IntegrationTests;

[Collection("BunkerGameApi")]
public class BunkersTests
{
    private readonly HttpClient _client;
    private readonly BunkerGameApiFixture _fixture;
    private readonly JsonSerializerOptions _jsonOptions;

    public BunkersTests(BunkerGameApiFixture fixture)
    {
        _client = fixture.CreateClient();
        _fixture = fixture;

        _jsonOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        _jsonOptions.Converters.Add(new JsonStringEnumConverter());
    }

    [Fact]
    public async Task GetBunker_WithValidGameSession_ReturnsBunkerData()
    {
        // Arrange
        var gameSessionResult = await CreateGameSession();

        // Act
        var response = await _client.GetAsync($"/api/game-sessions/{gameSessionResult.Id}/bunker");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var bunker = await response.Content.ReadFromJsonAsync<BunkerDto>(_jsonOptions);

        Assert.NotNull(bunker);
        Assert.Equal(gameSessionResult.Id, bunker.GameSessionId);
        Assert.NotEmpty(bunker.Description);
        Assert.NotNull(bunker.Rooms);
        Assert.NotNull(bunker.Items);
        Assert.NotNull(bunker.Environments);
    }

    [Fact]
    public async Task GetBunker_WithNonExistentGameSession_ReturnsNotFound()
    {
        // Arrange
        var nonExistentGameSessionId = Guid.NewGuid();

        // Act
        var response = await _client.GetAsync($"/api/game-sessions/{nonExistentGameSessionId}/bunker");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    private async Task<GameSessionCreationResult> CreateGameSession()
    {
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

        return createResult;
    }
}
