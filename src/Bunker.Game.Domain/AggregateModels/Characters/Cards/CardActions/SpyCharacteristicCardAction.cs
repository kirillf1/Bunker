using Bunker.Domain.Shared.CardActionCommands;

namespace Bunker.Game.Domain.AggregateModels.Characters.Cards.CardActions;

public class SpyCharacteristicCardAction : CardAction
{
    public CharacteristicType CharacteristicType { get; }
    public int TargetCharactersCount { get; }

    public SpyCharacteristicCardAction(
        CardActionRequirements cardActionRequirements,
        CharacteristicType characteristicType,
        int targetCharactersCount
    )
        : base(cardActionRequirements)
    {
        CharacteristicType = characteristicType;
        TargetCharactersCount = targetCharactersCount;
    }

    public override CardActionCommand CreateActionCommand(ActivateCardParams activateCardParams, Guid gameSessionId)
    {
        if (activateCardParams.TargetCharacterIds.Count() != TargetCharactersCount)
        {
            throw new ArgumentException("Invalid character count");
        }

        return new SpyCharacteristicActionCommand(CharacteristicType, activateCardParams.TargetCharacterIds);
    }

    public override CardActionRequirements GetCurrentCardActionRequirements()
    {
        return new CardActionRequirements(ActivateCardTargetType.None, 0);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return CardActionRequirements;
        yield return CharacteristicType;
        yield return TargetCharactersCount;
    }
}
