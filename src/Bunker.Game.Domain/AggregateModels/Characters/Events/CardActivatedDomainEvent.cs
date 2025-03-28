namespace Bunker.Game.Domain.AggregateModels.Characters.Events;

public class CardActivatedDomainEvent : IDomainEvent
{
    public Guid GameSessionId { get; }
    public Guid CharacterIdWhoActivateCard { get; }
    public string CardDescription { get; }

    public CardActivatedDomainEvent(Guid gameSessionId, Guid characterIdWhoActivateCard, string cardDescription)
    {
        GameSessionId = gameSessionId;
        CharacterIdWhoActivateCard = characterIdWhoActivateCard;
        CardDescription = cardDescription;
    }
}
