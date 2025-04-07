using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using Bunker.Domain.Shared.GameComponents;
using Bunker.GameComponents.API.Entities.CharacterComponents.Cards;
using Bunker.GameComponents.API.Entities.CharacterComponents.Cards.CardActions;
using Bunker.GameComponents.API.Infrastructure.Database;
using Bunker.GameComponents.API.Models.CharacterComponents.Cards;
using Bunker.GameComponents.Tests.Fixtures;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Bunker.GameComponents.Tests.FunctionalTests.CharacterComponents;

[Collection("BunkerComponentsApi")]
public class CardTests
{
    private readonly HttpClient _client;
    private readonly BunkerGameComponentsApiFixture _factory;
    private readonly JsonSerializerOptions _serializerOptions;

    public CardTests(BunkerGameComponentsApiFixture fixture)
    {
        _client = fixture.CreateClient();
        _factory = fixture;
        _serializerOptions = new JsonSerializerOptions();
        _serializerOptions.Converters.Add(new JsonStringEnumConverter());
        _serializerOptions.PropertyNameCaseInsensitive = true;
    }

    [Fact]
    public async Task GetAll_ReturnsSuccessAndListOfCards()
    {
        // Arrange
        using var scope = _factory.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<GameComponentsContext>();
        var card = new CardEntity("Test Card", new AddCharacteristicEntity(CharacteristicType.Health, null, 1));
        context.Cards.Add(card);
        await context.SaveChangesAsync();

        // Act
        var response = await _client.GetAsync("/api/character-components/cards");

        // Assert
        Assert.True(response.IsSuccessStatusCode);
        var cards = await response.Content.ReadFromJsonAsync<List<CardDto>>(_serializerOptions);
        Assert.NotNull(cards);
        Assert.Contains(cards, c => c.Description == "Test Card");
    }

    [Fact]
    public async Task GetById_ExistingId_ReturnsCard()
    {
        // Arrange
        using var scope = _factory.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<GameComponentsContext>();
        var card = new CardEntity("Test Card", new EmptyActionEntity()) { Id = Guid.NewGuid() };
        context.Cards.Add(card);
        await context.SaveChangesAsync();

        // Act
        var response = await _client.GetAsync($"/api/character-components/cards/{card.Id}");

        // Assert
        Assert.True(response.IsSuccessStatusCode);
        var dto = await response.Content.ReadFromJsonAsync<CardDto>(_serializerOptions);
        Assert.NotNull(dto);
        Assert.Equal(card.Id, dto.Id);
        Assert.Equal("Test Card", dto.Description);
        Assert.IsType<EmptyActionEntity>(dto.CardAction);
    }

    [Fact]
    public async Task GetById_NonExistingId_ReturnsNotFound()
    {
        // Act
        var response = await _client.GetAsync($"/api/character-components/cards/{Guid.NewGuid()}");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Theory]
    [InlineData(typeof(AddCharacteristicEntity), CharacteristicType.Health, 1)]
    [InlineData(typeof(EmptyActionEntity), null, 0)]
    [InlineData(typeof(ExchangeCharacteristicActionEntity), CharacteristicType.Profession, 0)]
    [InlineData(typeof(RecreateBunkerActionEntity), null, 0)]
    [InlineData(typeof(RecreateCatastropheActionEntity), null, 0)]
    [InlineData(typeof(RecreateCharacterActionEntity), null, 2)]
    [InlineData(typeof(RemoveCharacteristicCardActionEntity), CharacteristicType.Trait, 1)]
    [InlineData(typeof(RerollCharacteristicCardActionEntity), CharacteristicType.Hobby, 1)]
    [InlineData(typeof(RevealBunkerGameComponentCardActionEntity), null, 0)]
    [InlineData(typeof(SpyCharacteristicCardActionEntity), CharacteristicType.AdditionalInformation, 1)]
    public async Task Create_ValidCardAction_ReturnsCreated(
        Type actionType,
        CharacteristicType? characteristicType,
        int targetCount
    )
    {
        // Arrange
        var createDto = CreateCardDto(actionType, characteristicType, targetCount);

        // Act
        var response = await _client.PostAsJsonAsync("/api/character-components/cards", createDto);

        // Assert
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        var dto = await response.Content.ReadFromJsonAsync<CardDto>(_serializerOptions);
        Assert.NotNull(dto);
        Assert.Equal("Test Card", dto.Description);
        Assert.IsType(actionType, dto.CardAction);
        Assert.NotEqual(Guid.Empty, dto.Id);

        var location = response.Headers.Location?.ToString();
        Assert.Contains(dto.Id.ToString(), location);
    }

    [Theory]
    [InlineData(typeof(AddCharacteristicEntity), CharacteristicType.Health, 1)]
    [InlineData(typeof(ExchangeCharacteristicActionEntity), CharacteristicType.Profession, 0)]
    [InlineData(typeof(RemoveCharacteristicCardActionEntity), CharacteristicType.CharacterItem, 1)]
    public async Task Update_ExistingCard_UpdatesSuccessfully(
        Type actionType,
        CharacteristicType? characteristicType,
        int targetCount
    )
    {
        // Arrange
        using var scope = _factory.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<GameComponentsContext>();
        var cardAction = new EmptyActionEntity() { };
        var card = new CardEntity("Old Card", cardAction);
        context.Cards.Add(card);
        await context.SaveChangesAsync();

        var updateDto = new CardUpdateDto
        {
            Description = "Updated Card",
            CardAction = CreateCardActionDto(actionType, characteristicType, targetCount),
        };

        // Act
        var response = await _client.PutAsJsonAsync($"/api/character-components/cards/{card.Id}", updateDto);

        // Assert
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

        context.ChangeTracker.Clear();
        var updatedCard = await context.Cards.FirstOrDefaultAsync(c => c.Id == card.Id);
        Assert.NotNull(updatedCard);
        Assert.Equal("Updated Card", updatedCard.Description);
        Assert.IsType(actionType, updatedCard.CardAction);
    }

    [Fact]
    public async Task Delete_ExistingId_ReturnsNoContent()
    {
        // Arrange
        using var scope = _factory.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<GameComponentsContext>();
        var card = new CardEntity("Test Card", new EmptyActionEntity()) { Id = Guid.NewGuid() };
        context.Cards.Add(card);
        await context.SaveChangesAsync();

        // Act
        var response = await _client.DeleteAsync($"/api/character-components/cards/{card.Id}");

        // Assert
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

        context.ChangeTracker.Clear();
        var deletedCard = await context.Cards.FirstOrDefaultAsync(c => c.Id == card.Id);
        Assert.Null(deletedCard);
    }

    private CardCreateDto CreateCardDto(Type actionType, CharacteristicType? characteristicType, int targetCount)
    {
        return new CardCreateDto
        {
            Description = "Test Card",
            CardAction = CreateCardActionDto(actionType, characteristicType, targetCount),
        };
    }

    private CardActionEntity CreateCardActionDto(
        Type actionType,
        CharacteristicType? characteristicType,
        int targetCount
    )
    {
        return actionType switch
        {
            Type t when t == typeof(AddCharacteristicEntity) => new AddCharacteristicEntity(
                characteristicType!.Value,
                null, // CharacteristicId по умолчанию null, если не указано
                targetCount
            ),
            Type t when t == typeof(EmptyActionEntity) => new EmptyActionEntity(),
            Type t when t == typeof(ExchangeCharacteristicActionEntity) => new ExchangeCharacteristicActionEntity(
                characteristicType!.Value
            ),
            Type t when t == typeof(RecreateBunkerActionEntity) => new RecreateBunkerActionEntity(),
            Type t when t == typeof(RecreateCatastropheActionEntity) => new RecreateCatastropheActionEntity(),
            Type t when t == typeof(RecreateCharacterActionEntity) => new RecreateCharacterActionEntity(targetCount),
            Type t when t == typeof(RemoveCharacteristicCardActionEntity) => new RemoveCharacteristicCardActionEntity(
                characteristicType!.Value,
                targetCount
            ),
            Type t when t == typeof(RerollCharacteristicCardActionEntity) => new RerollCharacteristicCardActionEntity(
                characteristicType!.Value,
                false, // IsSelfTarget по умолчанию false
                null, // CharacteristicId по умолчанию null
                targetCount
            ),
            Type t when t == typeof(RevealBunkerGameComponentCardActionEntity) =>
                new RevealBunkerGameComponentCardActionEntity(
                    BunkerObjectType.BunkerItem // Значение по умолчанию из оригинального кода
                ),
            Type t when t == typeof(SpyCharacteristicCardActionEntity) => new SpyCharacteristicCardActionEntity(
                characteristicType!.Value,
                targetCount
            ),
            _ => throw new ArgumentException($"Unsupported action type: {actionType}"),
        };
    }
}
