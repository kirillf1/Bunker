namespace Bunker.Game.Domain.AggregateModels.Characters.Events;

public record CharacteristicRevealedDomainEvent(
    Guid CharacterId,
    Guid GameSessionId,
    IEnumerable<ICharacteristic> RevealedCharacteristics
) : IDomainEvent;
