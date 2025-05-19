using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using Bunker.Game.API.Models.Characters;
using Bunker.Game.API.Models.GameSessions;
using Bunker.Game.Application.Commands.GameSessions.CreateGameSession;
using Bunker.Game.Application.Queries.Characters.Models;
using Bunker.Game.Application.Queries.GameSessions;
using Bunker.Game.Domain.AggregateModels.Characters.Cards;
using Bunker.Game.Domain.AggregateModels.GameSessions;
using Bunker.Game.Tests.Fixtures;

namespace Bunker.Game.Tests.FunctionalTests.GameSessions;

[Collection("BunkerGameApi")]
public class UseCardTests
{
    private readonly HttpClient _client;
    private readonly BunkerGameApiFixture _fixture;
    private readonly JsonSerializerOptions _jsonOptions;

    public UseCardTests(BunkerGameApiFixture fixture)
    {
        _client = fixture.CreateClient();
        _fixture = fixture;

        _jsonOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        _jsonOptions.Converters.Add(new JsonStringEnumConverter());
    }

    [Fact]
    public async Task UseCard_WithValidRequest_ReturnsSuccess()
    {
        // Arrange
        var createResult = await CreateGameSession();
        var occupiedCharacterIds = await FillAllCharacters(createResult.Id, createResult.OccupiedCharacterId);

        var character = await GetCharacter(createResult.OccupiedCharacterId);
        Assert.NotEmpty(character.Cards);

        var cardToUse = character.Cards.First();
        var requirements = await GetCardRequirements(createResult.OccupiedCharacterId, cardToUse.Id);

        var useCardRequest = new UseCardRequest();
        if (requirements.Count > 0 && requirements.ActivateCardTargetType == ActivateCardTargetType.Character)
        {
            useCardRequest.TargetCharactersId = occupiedCharacterIds
                .Where(id => id != createResult.OccupiedCharacterId)
                .Take(requirements.Count)
                .ToList();
        }

        // Act
        var useCardResponse = await _client.PostAsJsonAsync(
            $"/api/characters/{createResult.OccupiedCharacterId}/cards/{cardToUse.Id}/use",
            useCardRequest,
            _jsonOptions
        );

        // Assert
        Assert.True(useCardResponse.IsSuccessStatusCode);

        var updatedCharacter = await GetCharacter(createResult.OccupiedCharacterId);
        var usedCard = updatedCharacter.Cards.FirstOrDefault(c => c.Id == cardToUse.Id);
        Assert.NotNull(usedCard);
        Assert.True(usedCard.IsActivated);
    }

    [Fact]
    public async Task UseCard_WithNonExistentCharacter_ReturnsBadRequest()
    {
        // Arrange
        var createResult = await CreateGameSession();
        var nonExistentCharacterId = Guid.NewGuid();
        var cardId = Guid.NewGuid();
        var useCardRequest = new UseCardRequest();

        // Act
        var response = await _client.PostAsJsonAsync(
            $"/api/characters/{nonExistentCharacterId}/cards/{cardId}/use",
            useCardRequest,
            _jsonOptions
        );

        // Assert
        Assert.False(response.IsSuccessStatusCode);
    }

    [Fact]
    public async Task UseCard_WithNonExistentCard_ReturnsBadRequest()
    {
        // Arrange
        var createResult = await CreateGameSession();
        var nonExistentCardId = Guid.NewGuid();
        var useCardRequest = new UseCardRequest();

        // Act
        var response = await _client.PostAsJsonAsync(
            $"/api/characters/{createResult.OccupiedCharacterId}/cards/{nonExistentCardId}/use",
            useCardRequest,
            _jsonOptions
        );

        // Assert
        Assert.False(response.IsSuccessStatusCode);
    }

    [Fact]
    public async Task UseCard_WithMissingRequiredTargets_ReturnsBadRequest()
    {
        // Arrange
        var createResult = await CreateGameSession();
        await FillAllCharacters(createResult.Id, createResult.OccupiedCharacterId);

        var character = await GetCharacter(createResult.OccupiedCharacterId);

        if (character.Cards.Count != 0)
        {
            var cardToUse = character.Cards.First();
            var requirements = await GetCardRequirements(character.Id, cardToUse.Id);

            if (
                requirements is not null
                && requirements.Count > 0
                && requirements.ActivateCardTargetType == ActivateCardTargetType.Character
            )
            {
                var useCardRequest = new UseCardRequest();

                // Act
                var response = await _client.PostAsJsonAsync(
                    $"/api/characters/{character.Id}/cards/{cardToUse.Id}/use",
                    useCardRequest,
                    _jsonOptions
                );

                // Assert
                Assert.False(response.IsSuccessStatusCode);
            }
        }
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

    private async Task<List<Guid>> FillAllCharacters(Guid gameSessionId, Guid creatorCharacterId)
    {
        var occupiedCharacterIds = new List<Guid> { creatorCharacterId };

        var gameSessionResponse = await _client.GetAsync($"/api/game-sessions/{gameSessionId}");
        var gameSession = await gameSessionResponse.Content.ReadFromJsonAsync<GameSessionDto>(_jsonOptions);
        Assert.NotNull(gameSession);

        var freeCharactersCount = gameSession.Characters.Count(c => !c.IsOccupiedByPlayer);
        for (int i = 0; i < freeCharactersCount; i++)
        {
            var playerId = Guid.NewGuid().ToString();
            var playerName = Guid.NewGuid().ToString();

            var occupyRequest = new OccupyCharacterRequest { PlayerId = playerId, PlayerName = playerName };

            var occupyResponse = await _client.PostAsJsonAsync(
                $"/api/game-sessions/{gameSessionId}/occupy-character",
                occupyRequest,
                _jsonOptions
            );

            Assert.True(occupyResponse.IsSuccessStatusCode);
            var characterId = await occupyResponse.Content.ReadFromJsonAsync<Guid>(_jsonOptions);
            occupiedCharacterIds.Add(characterId);
        }

        gameSessionResponse = await _client.GetAsync($"/api/game-sessions/{gameSessionId}");
        gameSession = await gameSessionResponse.Content.ReadFromJsonAsync<GameSessionDto>(_jsonOptions);
        Assert.NotNull(gameSession);
        Assert.Equal(GameState.Playing, gameSession.GameState);

        return occupiedCharacterIds;
    }

    private async Task<CharacterDto> GetCharacter(Guid characterId)
    {
        var characterResponse = await _client.GetAsync($"/api/characters/{characterId}");
        Assert.True(characterResponse.IsSuccessStatusCode);
        var character = await characterResponse.Content.ReadFromJsonAsync<CharacterDto>(_jsonOptions);
        Assert.NotNull(character);
        return character;
    }

    private async Task<CardActionRequirements> GetCardRequirements(Guid characterId, Guid cardId)
    {
        var requirementsResponse = await _client.GetAsync($"/api/characters/{characterId}/cards/{cardId}/requirements");
        Assert.True(requirementsResponse.IsSuccessStatusCode);
        var requirements = await requirementsResponse.Content.ReadFromJsonAsync<CardActionRequirements>(_jsonOptions);
        Assert.NotNull(requirements);
        return requirements;
    }
}
