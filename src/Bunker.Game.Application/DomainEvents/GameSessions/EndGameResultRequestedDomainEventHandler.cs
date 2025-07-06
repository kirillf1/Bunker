using Bunker.Application.Shared.Contracts.IntegrationEvents.GameResults;
using Bunker.Application.Shared.Contracts.IntegrationEvents.GameResults.GameComponents;
using Bunker.Game.Application.IntegrationEvents;
using Bunker.Game.Domain.AggregateModels.Bunkers;
using Bunker.Game.Domain.AggregateModels.Catastrophes;
using Bunker.Game.Domain.AggregateModels.Characters;
using Bunker.Game.Domain.AggregateModels.GameSessions.Events;
using Bunker.MessageBus.Abstractions;
using CharacterDomain = Bunker.Game.Domain.AggregateModels.Characters;
using GameSessionDomain = Bunker.Game.Domain.AggregateModels.GameSessions;

namespace Bunker.Game.Application.DomainEvents.GameSessions;

public class EndGameResultRequestedDomainEventHandler : DomainEventHandlerBase<EndGameResultRequestedDomainEvent>
{
    private readonly ILogger<EndGameResultRequestedDomainEventHandler> _logger;
    private readonly IBunkerRepository _bunkerRepository;
    private readonly ICatastropheRepository _catastropheRepository;
    private readonly ICharacterRepository _characterRepository;
    private readonly IBunkerGameIntegrationEventService _bunkerGameIntegrationEventService;

    public EndGameResultRequestedDomainEventHandler(
        ILogger<EndGameResultRequestedDomainEventHandler> logger,
        IBunkerRepository bunkerRepository,
        ICatastropheRepository catastropheRepository,
        ICharacterRepository characterRepository,
        IBunkerGameIntegrationEventService bunkerGameIntegrationEventService
    )
    {
        _logger = logger;
        _bunkerRepository = bunkerRepository;
        _catastropheRepository = catastropheRepository;
        _characterRepository = characterRepository;
        _bunkerGameIntegrationEventService = bunkerGameIntegrationEventService;
    }

    public override async Task Handle(
        EndGameResultRequestedDomainEvent domainEvent,
        CancellationToken cancellationToken
    )
    {
        _logger.LogInformation(
            "Processing EndGameResultRequestedDomainEvent for GameSession {GameSessionId}",
            domainEvent.GameSessionId
        );

        try
        {
            var bunker = await _bunkerRepository.GetBunkerByGameSessionId(domainEvent.GameSessionId);
            var catastrophe = await _catastropheRepository.GetCatastropheByGameSession(domainEvent.GameSessionId);
            var characters = await _characterRepository.GetCharactersByGameSession(domainEvent.GameSessionId);

            if (bunker is null)
            {
                _logger.LogWarning("Bunker not found for GameSession {GameSessionId}", domainEvent.GameSessionId);
                return;
            }

            if (catastrophe is null)
            {
                _logger.LogWarning("Catastrophe not found for GameSession {GameSessionId}", domainEvent.GameSessionId);
                return;
            }

            var notKickedCharacterIds = domainEvent.NotKickedCharacters.Select(x => x.Id);

            var notKickedCharacters = characters.Where(x => notKickedCharacterIds.Contains(x.Id));

            var integrationEvent = new GameResultRequestedIntegrationEvent
            {
                GameSessionId = domainEvent.GameSessionId,
                Bunker = MapBunkerData(bunker),
                Catastrophe = MapCatastropheData(catastrophe),
                Characters = MapCharactersData(notKickedCharacters, domainEvent.NotKickedCharacters),
            };

            await _bunkerGameIntegrationEventService.AddAndSaveEventAsync(integrationEvent);
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Failed to process EndGameResultRequestedDomainEvent for GameSession {GameSessionId}",
                domainEvent.GameSessionId
            );
            throw;
        }
    }

    private static BunkerData MapBunkerData(BunkerAggregate bunker)
    {
        return new BunkerData(
            bunker.Id,
            bunker.Description,
            bunker.Rooms.Select(x => new BunkerRoomData(x.GetDescription())),
            bunker.Items.Select(x => new BunkerItemData(x.GetDescription())),
            bunker.Environments.Select(x => new BunkerEnvironmentData(x.GetDescription()))
        );
    }

    private static CatastropheData MapCatastropheData(Catastrophe catastrophe)
    {
        return new CatastropheData(catastrophe.Description);
    }

    private static IEnumerable<CharacterData> MapCharactersData(
        IEnumerable<CharacterDomain.Character> characters,
        IEnumerable<GameSessionDomain.Character> sessionCharacters
    )
    {
        var sessionCharacterDict = sessionCharacters.ToDictionary(x => x.Id, x => x);

        return characters.Select(x => new CharacterData(
            x.Id,
            sessionCharacterDict.TryGetValue(x.Id, out var sessionChar)
                ? sessionChar.Player?.Name ?? "Unknown"
                : "Unknown",
            x.AdditionalInformation.GetDescription(),
            x.Age.Years,
            x.Childbearing.CanGiveBirth,
            x.Health.GetDescription(),
            x.Phobia.GetDescription(),
            x.Sex.GetDescription(),
            x.Hobby.GetDescription(),
            x.Hobby.Experience,
            x.Profession.GetDescription(),
            x.Profession.ExperienceYears,
            x.Size.Height,
            x.Size.Weight,
            x.Items.Select(i => new CharacterItemData(i.GetDescription())),
            x.Traits.Select(t => new CharacterTraitData(t.GetDescription()))
        ));
    }
}
