using Bunker.Application.Shared.Contracts.IntegrationEvents.GameResults;
using Bunker.Application.Shared.Contracts.IntegrationEvents.GameResults.GameComponents;
using Bunker.MessageBus.Abstractions;
using Bunker.ResultCreator.API.Application.IntegrationEvents.GameResultRequested;
using Bunker.ResultCreator.Tests.Fixtures;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Bunker.ResultCreator.Tests.IntegrationTests;

[Collection("BunkerResultCreatorApi")]
public class GameResultRequestedIntegrationEventHandlerTests
{
    private readonly BunkerResultCreatorApiFixture _fixture;
    private readonly ILogger<GameResultRequestedIntegrationEventHandlerTests> _logger;

    public GameResultRequestedIntegrationEventHandlerTests(BunkerResultCreatorApiFixture fixture)
    {
        _fixture = fixture;
        _logger = _fixture.Services.GetRequiredService<ILogger<GameResultRequestedIntegrationEventHandlerTests>>();
    }

    [Fact]
    public async Task Handle_WithValidGameResultRequestedEvent_ShouldProcessSuccessfully()
    {
        // Arrange
        using var scope = _fixture.Services.CreateScope();
        var handlers = scope.ServiceProvider.GetKeyedServices<
            IIntegrationEventHandler<GameResultRequestedIntegrationEvent>
        >(typeof(GameResultRequestedIntegrationEvent));

        var gameSessionId = Guid.NewGuid();
        var integrationEvent = new GameResultRequestedIntegrationEvent
        {
            GameSessionId = gameSessionId,
            Bunker = new BunkerData(
                Id: Guid.NewGuid(),
                Description: "Тестовый бункер",
                Rooms: new[] { new BunkerRoomData("Комната 1"), new BunkerRoomData("Комната 2") },
                Items: new[] { new BunkerItemData("Предмет 1"), new BunkerItemData("Предмет 2") },
                Environments: new[] { new BunkerEnvironmentData("Окружение 1") }
            ),
            Catastrophe = new CatastropheData("Тестовая катастрофа"),
            Characters = new[]
            {
                new CharacterData(
                    Id: Guid.NewGuid(),
                    Name: "Тестовый персонаж",
                    AdditionalInformation: "Дополнительная информация",
                    Age: 30,
                    CanGiveBirth: true,
                    Health: "Здоровый",
                    Phobia: "Нет фобий",
                    Sex: "Женский",
                    HobbyDescription: "Чтение",
                    HobbyExperience: 5,
                    ProfessionDescription: "Врач",
                    ProfessionExperienceYears: 10,
                    Height: 175.0,
                    Weight: 65.0,
                    Items: new[] { new CharacterItemData("Предмет персонажа") },
                    Traits: new[] { new CharacterTraitData("Черта характера") }
                ),
            },
        };

        // Act & Assert
        var exception = await Record.ExceptionAsync(async () =>
        {
            foreach (var handler in handlers)
            {
                await handler.Handle(integrationEvent);
            }
        });

        // Проверяем, что обработка прошла без исключений
        Assert.Null(exception);
    }

    [Fact]
    public async Task Handle_WithMinimalValidData_ShouldProcessSuccessfully()
    {
        // Arrange
        using var scope = _fixture.Services.CreateScope();
        var handlers = scope.ServiceProvider.GetKeyedServices<
            IIntegrationEventHandler<GameResultRequestedIntegrationEvent>
        >(typeof(GameResultRequestedIntegrationEvent));

        var gameSessionId = Guid.NewGuid();
        var integrationEvent = new GameResultRequestedIntegrationEvent
        {
            GameSessionId = gameSessionId,
            Bunker = new BunkerData(
                Id: Guid.NewGuid(),
                Description: "Минимальный бункер",
                Rooms: Array.Empty<BunkerRoomData>(),
                Items: Array.Empty<BunkerItemData>(),
                Environments: Array.Empty<BunkerEnvironmentData>()
            ),
            Catastrophe = new CatastropheData("Минимальная катастрофа"),
            Characters = Array.Empty<CharacterData>(),
        };

        // Act & Assert
        var exception = await Record.ExceptionAsync(async () =>
        {
            foreach (var handler in handlers)
            {
                await handler.Handle(integrationEvent);
            }
        });

        // Проверяем, что обработка прошла без исключений
        Assert.Null(exception);
    }
}
