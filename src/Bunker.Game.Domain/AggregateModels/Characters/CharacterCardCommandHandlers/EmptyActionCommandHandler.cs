using Bunker.Domain.Shared.CardActionCommands;

namespace Bunker.Game.Domain.AggregateModels.Characters.CharacterCardCommandHandlers;

public class EmptyActionCommandHandler : ICardActionCommandHandler<EmptyActionCommand>
{
    public Task Handle(EmptyActionCommand command)
    {
        return Task.CompletedTask;
    }
}
