using Bunker.Game.Domain.AggregateModels.Bunkers;
using Bunker.Game.Domain.AggregateModels.Catastrophes;
using Bunker.Game.Domain.AggregateModels.GameSessions.Events;

namespace Bunker.Game.Application.DomainEvents.GameSessions;

public class GameSessionCompletedDomainEventHandler : DomainEventHandlerBase<GameSessionCompletedDomainEvent>
{
    private readonly ILogger<GameSessionCompletedDomainEventHandler> _logger;
    private readonly IBunkerRepository _bunkerRepository;
    private readonly ICatastropheRepository _catastropheRepository;

    public GameSessionCompletedDomainEventHandler(
        ILogger<GameSessionCompletedDomainEventHandler> logger,
        IBunkerRepository bunkerRepository,
        ICatastropheRepository catastropheRepository
    )
    {
        _logger = logger;
        _bunkerRepository = bunkerRepository;
        _catastropheRepository = catastropheRepository;
    }

    public override async Task Handle(GameSessionCompletedDomainEvent domainEvent, CancellationToken cancel)
    {
        await MarkBunkerReadonly(domainEvent);

        await MarkCatastropheReadonly(domainEvent, cancel);

        _logger.LogInformation(
            "Game session {GameSessionId} completed with reason: {Reason}",
            domainEvent.GameSessionId,
            domainEvent.CompleteReason
        );
    }

    private async Task MarkCatastropheReadonly(GameSessionCompletedDomainEvent domainEvent, CancellationToken cancel)
    {
        var catastrophe = await _catastropheRepository.GetCatastropheByGameSession(domainEvent.GameSessionId);

        if (catastrophe is null)
        {
            _logger.LogWarning(
                "Game session complete but catastrophe is not found. Game session: {GameSessionId}",
                domainEvent.GameSessionId
            );
        }
        else
        {
            catastrophe.MarkReadOnly();

            await _catastropheRepository.Update(catastrophe);
            await _catastropheRepository.UnitOfWork.SaveChangesAsync(cancel);
        }
    }

    private async Task MarkBunkerReadonly(GameSessionCompletedDomainEvent domainEvent)
    {
        var bunker = await _bunkerRepository.GetBunkerByGameSessionId(domainEvent.GameSessionId);

        if (bunker is null)
        {
            _logger.LogWarning(
                "Game session complete but bunker is not found. Game session: {GameSessionId}",
                domainEvent.GameSessionId
            );
        }
        else
        {
            bunker.MarkReadonly();

            await _bunkerRepository.Update(bunker);
            await _bunkerRepository.UnitOfWork.SaveChangesAsync();
        }
    }
}
