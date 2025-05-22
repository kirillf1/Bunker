using Bunker.Game.Domain.AggregateModels.Characters;
using Bunker.Game.Domain.AggregateModels.GameSessions.Events;

namespace Bunker.Game.Application.DomainEvents.GameSessions;

public class CharacterKickedDomainEventHandler : DomainEventHandlerBase<CharacterKickedDomainEvent>
{
    private readonly ILogger<CharacterKickedDomainEventHandler> _logger;
    private readonly ICharacterRepository _characterRepository;

    public CharacterKickedDomainEventHandler(
        ILogger<CharacterKickedDomainEventHandler> logger,
        ICharacterRepository characterRepository
    )
    {
        _logger = logger;
        _characterRepository = characterRepository;
    }

    public override async Task Handle(CharacterKickedDomainEvent domainEvent, CancellationToken cancel)
    {
        var character = await _characterRepository.GetCharacter(domainEvent.CharacterId);

        if (character is null)
        {
            _logger.LogWarning(
                "Character {CharacterId} not found in repository while processing kick event",
                domainEvent.CharacterId
            );
            return;
        }

        character.MarkKicked();

        await _characterRepository.Update(character);

        await _characterRepository.UnitOfWork.SaveChangesAsync(cancel);

        _logger.LogInformation(
            "Character {CharacterId} was kicked from game session {GameSessionId}. Player {PlayerId} was removed from the game.",
            domainEvent.CharacterId,
            domainEvent.GameSessionId,
            domainEvent.PlayerId
        );
    }
}
