using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using Bunker.Domain.Shared.GameComponents;
using Bunker.GameComponents.API.Entities.CharacterComponents.Cards;
using Bunker.GameComponents.API.Entities.CharacterComponents.Cards.CardActions;
using Bunker.GameComponents.API.Infrastructure.Database;
using Bunker.GameComponents.API.Models.CharacterComponents.Cards;
using Bunker.GameComponents.API.Models.CharacterComponents.Cards.CardActions;
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
        var response = await _client.GetAsync("/api/cards");

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
        var response = await _client.GetAsync($"/api/cards/{card.Id}");

        // Assert
        Assert.True(response.IsSuccessStatusCode);
        var dto = await response.Content.ReadFromJsonAsync<CardDto>(_serializerOptions);
        Assert.NotNull(dto);
        Assert.Equal(card.Id, dto.Id);
        Assert.Equal("Test Card", dto.Description);
        Assert.IsType<EmptyActionDto>(dto.CardAction);
    }

    [Fact]
    public async Task GetById_NonExistingId_ReturnsNotFound()
    {
        // Act
        var response = await _client.GetAsync($"/api/cards/{Guid.NewGuid()}");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Theory]
    [InlineData(typeof(AddCharacteristicDto), CharacteristicType.Health, 1)]
    [InlineData(typeof(EmptyActionDto), null, 0)]
    [InlineData(typeof(ExchangeCharacteristicActionDto), CharacteristicType.Profession, 0)]
    [InlineData(typeof(RecreateBunkerActionDto), null, 0)]
    [InlineData(typeof(RecreateCatastropheActionDto), null, 0)]
    [InlineData(typeof(RecreateCharacterActionDto), null, 2)]
    [InlineData(typeof(RemoveCharacteristicCardActionDto), CharacteristicType.Trait, 1)]
    [InlineData(typeof(RerollCharacteristicCardActionDto), CharacteristicType.Hobby, 1)]
    [InlineData(typeof(RevealBunkerGameComponentCardActionDto), null, 0)]
    [InlineData(typeof(SpyCharacteristicCardActionDto), CharacteristicType.AdditionalInformation, 1)]
    public async Task Create_ValidCardAction_ReturnsCreated(
        Type actionType,
        CharacteristicType? characteristicType,
        int targetCount
    )
    {
        // Arrange
        var createDto = CreateCardDto(actionType, characteristicType, targetCount);

        // Act
        var response = await _client.PostAsJsonAsync("/api/cards", createDto);

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
    [InlineData(typeof(AddCharacteristicDto), CharacteristicType.Health, 1)]
    [InlineData(typeof(ExchangeCharacteristicActionDto), CharacteristicType.Profession, 0)]
    [InlineData(typeof(RemoveCharacteristicCardActionDto), CharacteristicType.CharacterItem, 1)]
    public async Task Update_ExistingCard_UpdatesSuccessfully(
        Type actionType,
        CharacteristicType? characteristicType,
        int targetCount
    )
    {
        // Arrange
        using var scope = _factory.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<GameComponentsContext>();
        var cardAction = new EmptyActionEntity() { Id = Guid.NewGuid() };
        var card = new CardEntity("Old Card", cardAction);
        context.Cards.Add(card);
        await context.SaveChangesAsync();

        var updateDto = new CardUpdateDto
        {
            Description = "Updated Card",
            CardAction = CreateCardActionDto(actionType, characteristicType, targetCount, cardAction.Id),
        };

        // Act
        var response = await _client.PutAsJsonAsync($"/api/cards/{card.Id}", updateDto);

        // Assert
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

        context.ChangeTracker.Clear();
        var updatedCard = await context.Cards.Include(c => c.CardAction).FirstOrDefaultAsync(c => c.Id == card.Id);
        Assert.NotNull(updatedCard);
        Assert.Equal("Updated Card", updatedCard.Description);
        Assert.IsType(GetEntityTypeFromDtoType(actionType), updatedCard.CardAction);
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
        var response = await _client.DeleteAsync($"/api/cards/{card.Id}");

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

    private CardActionDto CreateCardActionDto(
        Type actionType,
        CharacteristicType? characteristicType,
        int targetCount,
        Guid? id = null
    )
    {
        return actionType switch
        {
            Type t when t == typeof(AddCharacteristicDto) => new AddCharacteristicDto
            {
                CharacteristicType = characteristicType!.Value,
                TargetCharactersCount = targetCount,
                Id = id ?? Guid.CreateVersion7(),
            },
            Type t when t == typeof(EmptyActionDto) => new EmptyActionDto() { Id = id ?? Guid.CreateVersion7() },
            Type t when t == typeof(ExchangeCharacteristicActionDto) => new ExchangeCharacteristicActionDto
            {
                CharacteristicType = characteristicType!.Value,
                Id = id ?? Guid.CreateVersion7(),
            },
            Type t when t == typeof(RecreateBunkerActionDto) => new RecreateBunkerActionDto()
            {
                Id = id ?? Guid.CreateVersion7(),
            },
            Type t when t == typeof(RecreateCatastropheActionDto) => new RecreateCatastropheActionDto()
            {
                Id = id ?? Guid.CreateVersion7(),
            },
            Type t when t == typeof(RecreateCharacterActionDto) => new RecreateCharacterActionDto
            {
                TargetCharactersCount = targetCount,
                Id = id ?? Guid.CreateVersion7(),
            },
            Type t when t == typeof(RemoveCharacteristicCardActionDto) => new RemoveCharacteristicCardActionDto
            {
                CharacteristicType = characteristicType!.Value,
                TargetCharactersCount = targetCount,
                Id = id ?? Guid.CreateVersion7(),
            },
            Type t when t == typeof(RerollCharacteristicCardActionDto) => new RerollCharacteristicCardActionDto
            {
                CharacteristicType = characteristicType!.Value,
                TargetCharactersCount = targetCount,
                IsSelfTarget = false,
                Id = id ?? Guid.CreateVersion7(),
            },
            Type t when t == typeof(RevealBunkerGameComponentCardActionDto) =>
                new RevealBunkerGameComponentCardActionDto
                {
                    BunkerObjectType = BunkerObjectType.BunkerItem,
                    Id = id ?? Guid.CreateVersion7(),
                },
            Type t when t == typeof(SpyCharacteristicCardActionDto) => new SpyCharacteristicCardActionDto
            {
                CharacteristicType = characteristicType!.Value,
                TargetCharactersCount = targetCount,
                Id = id ?? Guid.CreateVersion7(),
            },
            _ => throw new ArgumentException($"Unsupported action type: {actionType}"),
        };
    }

    private Type GetEntityTypeFromDtoType(Type dtoType)
    {
        return dtoType switch
        {
            Type t when t == typeof(AddCharacteristicDto) => typeof(AddCharacteristicEntity),
            Type t when t == typeof(EmptyActionDto) => typeof(EmptyActionEntity),
            Type t when t == typeof(ExchangeCharacteristicActionDto) => typeof(ExchangeCharacteristicActionEntity),
            Type t when t == typeof(RecreateBunkerActionDto) => typeof(RecreateBunkerActionEntity),
            Type t when t == typeof(RecreateCatastropheActionDto) => typeof(RecreateCatastropheActionEntity),
            Type t when t == typeof(RecreateCharacterActionDto) => typeof(RecreateCharacterActionEntity),
            Type t when t == typeof(RemoveCharacteristicCardActionDto) => typeof(RemoveCharacteristicCardActionEntity),
            Type t when t == typeof(RerollCharacteristicCardActionDto) => typeof(RerollCharacteristicCardActionEntity),
            Type t when t == typeof(RevealBunkerGameComponentCardActionDto) =>
                typeof(RevealBunkerGameComponentCardActionEntity),
            Type t when t == typeof(SpyCharacteristicCardActionDto) => typeof(SpyCharacteristicCardActionEntity),
            _ => throw new ArgumentException($"Unsupported DTO type: {dtoType}"),
        };
    }
}
