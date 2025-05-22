using Bunker.Game.Domain.AggregateModels.Bunkers;
using Bunker.Game.Domain.AggregateModels.Catastrophes;
using Bunker.Game.Domain.AggregateModels.GameSessions.Events;

namespace Bunker.Game.Application.DomainEvents.GameSessions;

public class GameSessionCreatedDomainEventHandler : DomainEventHandlerBase<GameSessionCreatedDomainEvent>
{
    private readonly ILogger<GameSessionCreatedDomainEventHandler> _logger;
    private readonly ICatastropheGenerator _catastropheGenerator;
    private readonly ICatastropheRepository _catastropheRepository;
    private readonly IBunkerGenerator _bunkerGenerator;
    private readonly IBunkerRepository _bunkerRepository;

    public GameSessionCreatedDomainEventHandler(
        ILogger<GameSessionCreatedDomainEventHandler> logger,
        ICatastropheGenerator catastropheGenerator,
        ICatastropheRepository catastropheRepository,
        IBunkerGenerator bunkerGenerator,
        IBunkerRepository bunkerRepository
    )
    {
        _logger = logger;
        _catastropheGenerator = catastropheGenerator;
        _catastropheRepository = catastropheRepository;
        _bunkerGenerator = bunkerGenerator;
        _bunkerRepository = bunkerRepository;
    }

    public override async Task Handle(GameSessionCreatedDomainEvent domainEvent, CancellationToken cancel)
    {
        await GenerateCatastrophe(domainEvent.GameSessionId, cancel);

        await GenerateBunker(domainEvent.GameSessionId, cancel);

        _logger.LogInformation(
            "Successfully generated catastrophe and bunker for GameSession {GameSessionId}",
            domainEvent.GameSessionId
        );
    }

    private async Task GenerateCatastrophe(Guid gameSessionId, CancellationToken cancel)
    {
        var catastrophe = await _catastropheGenerator.GenerateCatastrophe(gameSessionId);

        await _catastropheRepository.Add(catastrophe);
        await _catastropheRepository.UnitOfWork.SaveChangesAsync(cancel);
    }

    private async Task GenerateBunker(Guid gameSessionId, CancellationToken cancel)
    {
        var bunker = await _bunkerGenerator.GenerateBunker(gameSessionId);

        await _bunkerRepository.Add(bunker);
        await _bunkerRepository.UnitOfWork.SaveChangesAsync(cancel);
    }
}
