namespace Bunker.Domain.Shared.Cards.CardActionCommands;

public class RecreateCharacterActionCommand : CardActionCommand
{
    public IEnumerable<Guid> TargetCharactersIds { get; }

    public RecreateCharacterActionCommand(IEnumerable<Guid> targetCharactersIds)
    {
        TargetCharactersIds = targetCharactersIds;
    }
}
