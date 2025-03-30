using Bunker.Domain.Shared.Cards.CardActionCommands;

namespace Bunker.Game.Domain.AggregateModels.Characters.Cards;

public interface ICardCommandExecutor
{
    public Task ExecuteCardAction(CardActionCommand command);
}
