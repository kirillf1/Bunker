using Bunker.Game.Domain.AggregateModels.Characters.Cards.CardActions;

namespace Bunker.Domain.Shared.CardActionCommands;

public class RecreateCharacterActionCommand : CardActionCommand
{
    public IEnumerable<Guid> TargetCharactersIds { get; }

    public RecreateCharacterActionCommand(IEnumerable<Guid> targetCharactersIds)
    {
        TargetCharactersIds = targetCharactersIds;
    }
}
