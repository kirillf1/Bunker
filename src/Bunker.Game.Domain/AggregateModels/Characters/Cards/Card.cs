using Bunker.Domain.Shared.Cards.CardActionCommands;
using Bunker.Game.Domain.AggregateModels.Characters.Cards.CardActions;

namespace Bunker.Game.Domain.AggregateModels.Characters.Cards;

public class Card : Entity<Guid>, ICharacteristic
{
    public bool IsActivated { get; private set; }

    public string Description { get; private set; }

    public CardAction CardAction { get; private set; }
    public Guid SourceCardId { get; private set; }

    public Card(Guid cardId, string description, CardAction cardAction, Guid sourceCardId)
        : base(cardId)
    {
        Description = description;
        CardAction = cardAction;
        SourceCardId = sourceCardId;
        IsActivated = false;
    }

#pragma warning disable CS8618 // Поле, не допускающее значения NULL, должно содержать значение, отличное от NULL, при выходе из конструктора. Рассмотрите возможность добавления модификатора "required" или объявления значения, допускающего значение NULL.
    private Card(Guid id)
#pragma warning restore CS8618 // Поле, не допускающее значения NULL, должно содержать значение, отличное от NULL, при выходе из конструктора. Рассмотрите возможность добавления модификатора "required" или объявления значения, допускающего значение NULL.
        : base(id) { }

    public CardActionCommand ActivateCard(ActivateCardParams activateCardParams, Guid gameSessionId)
    {
        if (IsActivated)
        {
            throw new InvalidGameOperationException($"Card {Id} already activated");
        }

        var cardActionCommand = CardAction.CreateActionCommand(activateCardParams, gameSessionId);

        IsActivated = true;

        return cardActionCommand;
    }

    public string GetDescription()
    {
        return Description;
    }
}
