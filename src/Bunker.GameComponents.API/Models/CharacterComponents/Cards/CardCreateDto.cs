﻿using Bunker.GameComponents.API.Models.CharacterComponents.Cards.CardActions;

namespace Bunker.GameComponents.API.Models.CharacterComponents.Cards;

public class CardCreateDto
{
    public required string Description { get; set; }
    public required CardActionDto CardAction { get; set; }
}
