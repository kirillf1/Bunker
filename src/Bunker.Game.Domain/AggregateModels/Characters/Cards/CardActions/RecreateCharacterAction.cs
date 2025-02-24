using Bunker.Domain.Shared.CardActionCommands;

namespace Bunker.Game.Domain.AggregateModels.Characters.Cards.CardActions;

public class RecreateCharacterAction : CardAction
{
    public int TargetCharactersCount { get; }

    public RecreateCharacterAction(CardActionRequirements cardActionRequirements, int targetCharactersCount)
        : base(cardActionRequirements)
    {
        TargetCharactersCount = targetCharactersCount;
    }

    public override CardActionCommand CreateActionCommand(ActivateCardParams activateCardParams)
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
        yield return CardActionRequirements;
        yield return TargetCharactersCount;
    }
}
