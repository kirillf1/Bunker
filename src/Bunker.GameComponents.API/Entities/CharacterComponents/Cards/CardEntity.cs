﻿using Bunker.GameComponents.API.Entities.CharacterComponents.Cards.CardActions;

namespace Bunker.GameComponents.API.Entities.CharacterComponents.Cards;

public class CardEntity
{
    public Guid Id { get; set; }
    public string Description { get; set; }
    public CardActionEntity CardAction { get; set; }

#pragma warning disable CS8618 // Поле, не допускающее значения NULL, должно содержать значение, отличное от NULL, при выходе из конструктора. Рассмотрите возможность добавления модификатора "required" или объявления значения, допускающего значение NULL.
    public CardEntity() { }
#pragma warning restore CS8618 // Поле, не допускающее значения NULL, должно содержать значение, отличное от NULL, при выходе из конструктора. Рассмотрите возможность добавления модификатора "required" или объявления значения, допускающего значение NULL.

    public CardEntity(string description, CardActionEntity cardAction)
    {
        Id = Guid.CreateVersion7();
        Description = description;
        CardAction = cardAction;
    }
}
