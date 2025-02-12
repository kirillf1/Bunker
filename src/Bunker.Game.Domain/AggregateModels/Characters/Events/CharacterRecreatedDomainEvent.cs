using Bunker.Game.Domain.AggregateModels.Characters.Cards;
using Bunker.Game.Domain.AggregateModels.Characters.Characteristics;

namespace Bunker.Game.Domain.AggregateModels.Characters.Events;

public record CharacterRecreatedDomainEvent(
    Guid Id,
    Guid GameSessionId,
    AdditionalInformation AdditionalInformation,
    Age Age,
    Childbearing Childbearing,
    Health Health,
    Hobby Hobby,
    Phobia Phobia,
    Profession Profession,
    Sex Sex,
    IEnumerable<Item> Items,
    IEnumerable<Trait> Traits,
    IEnumerable<Card> Cards
) : IDomainEvent;
