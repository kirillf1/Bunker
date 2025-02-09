namespace Bunker.Game.Domain.AggregateModels.Characters.Cards.CardActions;

public class ExchangeCharacteristic : CardAction
{
    public CharacteristicType CharacteristicType { get; }

    public ExchangeCharacteristic(CardActionRequirements cardActionRequirements, CharacteristicType characteristicType)
        : base(cardActionRequirements)
    {
        CharacteristicType = characteristicType;
    }

    public override CardActionCommand CreateActionCommand(ActivateCardParams activateCardParams)
    {
        return new ExchangeCharacteristicActionCommand(CharacteristicType);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return CardActionRequirements;
        yield return CharacteristicType;
    }

    public class ExchangeCharacteristicActionCommand : CardActionCommand
    {
        public CharacteristicType CharacteristicType { get; }

        public ExchangeCharacteristicActionCommand(CharacteristicType characteristicType)
        {
            CharacteristicType = characteristicType;
        }
    }
}
