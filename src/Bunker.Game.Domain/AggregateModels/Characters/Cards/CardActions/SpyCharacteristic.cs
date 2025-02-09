namespace Bunker.Game.Domain.AggregateModels.Characters.Cards.CardActions;

public class SpyCharacteristic : CardAction
{
    public CharacteristicType CharacteristicType { get; }
    public int TargetCharactersCount { get; }

    public SpyCharacteristic(
        CardActionRequirements cardActionRequirements,
        CharacteristicType characteristicType,
        int targetCharactersCount
    )
        : base(cardActionRequirements)
    {
        CharacteristicType = characteristicType;
        TargetCharactersCount = targetCharactersCount;
    }

    public override CardActionCommand CreateActionCommand(ActivateCardParams activateCardParams)
    {
        if (activateCardParams.TargetCharacterIds.Count() != TargetCharactersCount)
        {
            throw new ArgumentException("Invalid character count");
        }

        return new SpyCharacteristicActionCommand(CharacteristicType, activateCardParams.TargetCharacterIds);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return CharacteristicType;
        yield return TargetCharactersCount;
    }

    public class SpyCharacteristicActionCommand : CardActionCommand
    {
        public CharacteristicType CharacteristicType { get; }
        public IEnumerable<Guid> TargetCharactersIds { get; }

        public SpyCharacteristicActionCommand(
            CharacteristicType characteristicType,
            IEnumerable<Guid> targetCharactersIds
        )
        {
            CharacteristicType = characteristicType;
            TargetCharactersIds = targetCharactersIds;
        }
    }
}
