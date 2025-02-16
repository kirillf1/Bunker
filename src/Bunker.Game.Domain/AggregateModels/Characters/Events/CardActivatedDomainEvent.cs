namespace Bunker.Game.Domain.AggregateModels.Characters.Events;

public class CardActivatedDomainEvent : IDomainEvent
{
    public Guid GameSessionId { get; }
    public Guid CharacterIdWhoActivatedCard { get; }
    public string CardDescription { get; }

    public CardActivatedDomainEvent(Guid gameSessionId, Guid characterIdWhoActivatedCard, string cardDescription)
    {
        GameSessionId = gameSessionId;
        CharacterIdWhoActivatedCard = characterIdWhoActivatedCard;
        CardDescription = cardDescription;
    }
}
