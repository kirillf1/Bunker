using Bunker.Domain.Shared.DomainEvents;
using Bunker.Game.Domain.AggregateModels.Characters;
using Bunker.Game.Domain.AggregateModels.GameSessions;
using Bunker.Game.Domain.AggregateModels.GameSessions.Events;
using Microsoft.Extensions.Logging;

namespace Bunker.Game.API.Application.DomainEvents.GameSessions;

public class GameSessionCompletedDomainEventHandler : DomainEventHandlerBase<GameSessionCompletedDomainEvent>
{
    private readonly ILogger<GameSessionCompletedDomainEventHandler> _logger;
    private readonly ICharacterRepository _characterRepository;

    public GameSessionCompletedDomainEventHandler(
        ILogger<GameSessionCompletedDomainEventHandler> logger,
        ICharacterRepository characterRepository
    )
    {
        _logger = logger;
        _characterRepository = characterRepository;
    }

    public override Task Handle(GameSessionCompletedDomainEvent domainEvent)
    {
        _logger.LogInformation(
            "Game session {GameSessionId} completed with reason: {Reason}",
            domainEvent.GameSessionId,
            domainEvent.CompleteReason
        );

        return Task.CompletedTask;
    }
}
