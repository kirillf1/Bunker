using Bunker.Game.Domain.AggregateModels.Characters.Cards.CardActions;

namespace Bunker.Game.Domain.AggregateModels.Characters.Events;

public class CardActionActivatedDomainEvent : IDomainEvent
{
    public Guid GameSessionId { get; }
    public Guid CharacterIdWhoActivatedCard { get; }
    public string CardDescription { get; }
    public CardActionCommand CardActionCommand { get; }

    public CardActionActivatedDomainEvent(
        Guid gameSessionId,
        Guid characterIdWhoActivatedCard,
        string cardDescription,
        CardActionCommand cardAction
    )
    {
        GameSessionId = gameSessionId;
        CharacterIdWhoActivatedCard = characterIdWhoActivatedCard;
        CardDescription = cardDescription;
        CardActionCommand = cardAction;
    }
}
