using Bunker.Domain.Shared.Cards.CardActionCommands;

namespace Bunker.Game.Domain.AggregateModels.Characters.Cards.CardActions;

public class RecreateCharacterAction : CardAction
{
    public int TargetCharactersCount { get; }

    public RecreateCharacterAction(int targetCharactersCount)
    {
        TargetCharactersCount = targetCharactersCount;
    }

    public override CardActionCommand CreateActionCommand(ActivateCardParams activateCardParams, Guid gameSessionId)
    {
        if (activateCardParams.TargetCharacterIds.Count() != TargetCharactersCount)
        {
            throw new ArgumentException("Invalid character count");
        }

        return new RecreateCharacterActionCommand(activateCardParams.TargetCharacterIds);
    }

    public override CardActionRequirements GetCurrentCardActionRequirements()
    {
        return new CardActionRequirements(ActivateCardTargetType.Character, TargetCharactersCount);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return TargetCharactersCount;
    }
}
