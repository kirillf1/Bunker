﻿using Bunker.Domain.Shared.Cards.CardActionCommands;
using Bunker.Game.Domain.AggregateModels.Characters.Cards.CardActions;

namespace Bunker.Game.Domain.AggregateModels.Characters.Cards;

public class Card : Entity<Guid>, ICharacteristic
{
    public bool IsActivated { get; private set; }

    public string Description { get; private set; }

    public CardAction CardAction { get; private set; }

    public Card(Guid cardId, string description, CardAction cardAction)
        : base(cardId)
    {
        Description = description;
        CardAction = cardAction;
        IsActivated = false;
    }

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
