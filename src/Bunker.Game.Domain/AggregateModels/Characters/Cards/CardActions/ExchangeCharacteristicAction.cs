using Bunker.Domain.Shared.CardActionCommands;

namespace Bunker.Game.Domain.AggregateModels.Characters.Cards.CardActions;

public class ExchangeCharacteristicAction : CardAction
{
    public CharacteristicType CharacteristicType { get; }

    public ExchangeCharacteristicAction(CardActionRequirements cardActionRequirements, CharacteristicType characteristicType)
        : base(cardActionRequirements)
    {
        CharacteristicType = characteristicType;
    }

    public override CardActionCommand CreateActionCommand(ActivateCardParams activateCardParams)
    {
        if (activateCardParams.TargetCharacterIds.Count() != 2)
        {
            throw new ArgumentException("For exchange characters must be 2");
        }
        return new ExchangeCharacteristicActionCommand(CharacteristicType, activateCardParams.TargetCharacterIds);
    }

    public override CardActionRequirements GetCurrentCardActionRequirements()
    {
        return new CardActionRequirements(ActivateCardTargetType.Character, 2);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return CardActionRequirements;
        yield return CharacteristicType;
    }
}
