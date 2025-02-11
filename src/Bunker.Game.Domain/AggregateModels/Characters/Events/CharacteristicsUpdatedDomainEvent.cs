namespace Bunker.Game.Domain.AggregateModels.Characters.Events;

public record CharacteristicsUpdatedDomainEvent(Guid CharacterId, IEnumerable<ICharacteristic> Characteristics)
    : IDomainEvent;
