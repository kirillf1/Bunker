using Bunker.Game.Domain.AggregateModels.Characters.Cards.CardActions;

namespace Bunker.Game.Domain.AggregateModels.Characters.Cards;

public interface ICardCommandExecutor
{
    public Task ExecuteCardAction(CardActionCommand command);
}
