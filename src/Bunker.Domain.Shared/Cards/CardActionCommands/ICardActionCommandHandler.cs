namespace Bunker.Domain.Shared.Cards.CardActionCommands;

public interface ICardActionCommandHandler<in T>
    where T : CardActionCommand
{
    public Task Handle(T command);
}
