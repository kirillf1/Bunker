using Bunker.Game.Domain.AggregateModels.Characters.Cards.CardActions;

namespace Bunker.Domain.Shared.CardActionCommands;

public interface ICardActionCommandHandler<in T>
    where T : CardActionCommand
{
    public Task Handle(T command);
}
