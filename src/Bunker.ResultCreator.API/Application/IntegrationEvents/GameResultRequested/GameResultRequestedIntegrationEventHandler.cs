using Ardalis.Result;
using Bunker.Application.Shared.Contracts.IntegrationEvents.GameResults;
using Bunker.Application.Shared.Contracts.IntegrationEvents.GameResults.GameComponents;
using Bunker.Application.Shared.CQRS;
using Bunker.MessageBus.Abstractions;
using Bunker.ResultCreator.API.Application.Commands.CreateGameResult;
using Bunker.ResultCreator.API.Domain.Bunkers;
using Bunker.ResultCreator.API.Domain.Catastrophes;
using Bunker.ResultCreator.API.Domain.Characters;

namespace Bunker.ResultCreator.API.Application.IntegrationEvents.GameResultRequested;

public class GameResultRequestedIntegrationEventHandler : IIntegrationEventHandler<GameResultRequestedIntegrationEvent>
{
    private readonly ICommandHandler<CreateGameResultCommand, Result<string>> _createGameResultCommandHandler;
    private readonly IMessageBus _messageBus;
    private readonly ILogger<GameResultRequestedIntegrationEventHandler> _logger;

    public GameResultRequestedIntegrationEventHandler(
        ICommandHandler<CreateGameResultCommand, Result<string>> createGameResultCommandHandler,
        IMessageBus messageBus,
        ILogger<GameResultRequestedIntegrationEventHandler> logger
    )
    {
        _createGameResultCommandHandler = createGameResultCommandHandler;
        _messageBus = messageBus;
        _logger = logger;
    }

    public async Task Handle(GameResultRequestedIntegrationEvent @event)
    {
        _logger.LogInformation(
            "Processing GameResultRequestedIntegrationEvent for GameSession {GameSessionId}",
            @event.GameSessionId
        );

        var command = new CreateGameResultCommand(
            @event.GameSessionId,
            MapBunkerData(@event.Bunker),
            MapCatastropheData(@event.Catastrophe),
            MapCharactersData(@event.Characters)
        );

        var result = await _createGameResultCommandHandler.Handle(command, CancellationToken.None);

        if (result.IsSuccess)
        {
            var responseEvent = new GameResultRespondedIntegrationEvent
            {
                GameSessionId = @event.GameSessionId,
                GameResultDescription = result.Value,
            };

            await _messageBus.PublishAsync(responseEvent);

            _logger.LogInformation(
                "Successfully processed GameResultRequestedIntegrationEvent and sent response for GameSession {GameSessionId}",
                @event.GameSessionId
            );
        }
        else
        {
            _logger.LogError(
                "Failed to create game result for GameSession {GameSessionId}: {Error}",
                @event.GameSessionId,
                string.Join(", ", result.Errors)
            );
        }
    }

    private static BunkerEntity MapBunkerData(BunkerData bunkerData)
    {
        return new BunkerEntity
        {
            Id = bunkerData.Id,
            Description = bunkerData.Description,
            Rooms = bunkerData.Rooms.Select(r => new BunkerRoom { Description = r.Description }).ToList(),
            Items = bunkerData.Items.Select(i => new BunkerItem { Description = i.Description }).ToList(),
            Environments = bunkerData
                .Environments.Select(e => new BunkerEnvironment { Description = e.Description })
                .ToList(),
        };
    }

    private static Catastrophe MapCatastropheData(CatastropheData catastropheData)
    {
        return new Catastrophe { Description = catastropheData.Description };
    }

    private static IEnumerable<Character> MapCharactersData(IEnumerable<CharacterData> charactersData)
    {
        return charactersData.Select(c => new Character
        {
            Id = c.Id,
            Name = c.Name,
            AdditionalInformation = c.AdditionalInformation,
            Age = c.Age,
            CanGiveBirth = c.CanGiveBirth,
            Health = c.Health,
            Phobia = c.Phobia,
            Sex = c.Sex,
            HobbyDescription = c.HobbyDescription,
            HobbyExperience = c.HobbyExperience,
            ProfessionDescription = c.ProfessionDescription,
            ProfessionExperienceYears = c.ProfessionExperienceYears,
            Height = c.Height,
            Weight = c.Weight,
            Items = c.Items.Select(i => new CharacterItem { Description = i.Description }).ToList(),
            Traits = c.Traits.Select(t => new CharacterTrait { Description = t.Description }).ToList(),
        });
    }
}
