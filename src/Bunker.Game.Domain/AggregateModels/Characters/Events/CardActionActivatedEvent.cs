using Bunker.Game.Domain.AggregateModels.Characters.Cards.CardActions;

namespace Bunker.Game.Domain.AggregateModels.Characters.Events;

public class CardActionActivatedEvent : IDomainEvent
{
    public Guid GameSessionId { get; }
    public Guid CharacterIdWhoActivatedCard { get; }
    public string CardDescription { get; }
    public CardActionCommand CardActionCommand { get; }

    protected CardActionActivatedEvent(
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
