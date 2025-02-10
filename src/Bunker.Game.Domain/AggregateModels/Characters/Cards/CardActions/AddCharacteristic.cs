namespace Bunker.Game.Domain.AggregateModels.Characters.Cards.CardActions;

public class AddCharacteristic : CardAction
{
    public CharacteristicType CharacteristicType { get; }
    public Guid? CharacteristicId { get; }
    public int TargetCharactersCount { get; }

    public AddCharacteristic(
        CardActionRequirements cardActionRequirements,
        CharacteristicType characteristicType,
        Guid? characteristicId,
        int targetCharactersCount
    )
        : base(cardActionRequirements)
    {
        CharacteristicType = characteristicType;
        CharacteristicId = characteristicId;
        TargetCharactersCount = targetCharactersCount;
    }

    public override CardActionCommand CreateActionCommand(ActivateCardParams activateCardParams)
    {
        if (activateCardParams.TargetCharacterIds.Count() != TargetCharactersCount)
        {
            throw new ArgumentException("Invalid character count");
        }

        return new AddCharacteristicActionCommand(
            CharacteristicType,
            activateCardParams.TargetCharacterIds,
            CharacteristicId
        );
    }

    public override CardActionRequirements GetCurrentCardActionRequirements()
    {
        return new CardActionRequirements(ActivateCardTargetType.Character, TargetCharactersCount);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return CharacteristicType;
        yield return CharacteristicId ?? Guid.Empty;
    }

    public class AddCharacteristicActionCommand : CardActionCommand
    {
        public CharacteristicType CharacteristicType { get; }
        public IEnumerable<Guid> TargetCharactersIds { get; }
        public Guid? CharacteristicId { get; }

        public AddCharacteristicActionCommand(
            CharacteristicType characteristicType,
            IEnumerable<Guid> targetCharactersIds,
            Guid? characteristicId
        )
        {
            CharacteristicType = characteristicType;
            TargetCharactersIds = targetCharactersIds;
            CharacteristicId = characteristicId;
        }
    }
}
