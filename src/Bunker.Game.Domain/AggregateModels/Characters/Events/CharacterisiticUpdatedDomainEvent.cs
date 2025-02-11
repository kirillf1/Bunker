namespace Bunker.Game.Domain.AggregateModels.Characters.Events;

public record CharacteristicUpdatedDomainEvent(Guid CharacterId, ICharacteristic Characteristic) : IDomainEvent;
