using Bunker.Domain.Shared.CardActionCommands;
using Bunker.Game.Domain.AggregateModels.Characters.Events;

namespace Bunker.Game.Domain.AggregateModels.Characters.CharacterCardCommandHandlers;

public class RecreateCharacterActionCommandHandler : ICardActionCommandHandler<RecreateCharacterActionCommand>
{
    public Task Handle(RecreateCharacterActionCommand command)
    {
        throw new NotImplementedException();
    }
}
