namespace Bunker.Game.Domain.AggregateModels.Characters.Cards.CardActions;

public class RerollCharacteristic : CardAction
{
    public CharacteristicType CharacteristicType { get; }
    public bool IsSelfTarget { get; }
    public Guid? CharacteristicId { get; }
    public int TargetCharactersCount { get; }

    public RerollCharacteristic(
        CardActionRequirements cardActionRequirements,
        CharacteristicType characteristicType,
        bool isSelfTarget,
        Guid? characteristicId,
        int targetCharactersCount
    )
        : base(cardActionRequirements)
    {
        CharacteristicType = characteristicType;
        IsSelfTarget = isSelfTarget;
        CharacteristicId = characteristicId;
        TargetCharactersCount = targetCharactersCount;
    }

    public override CardActionCommand CreateActionCommand(ActivateCardParams activateCardParams)
    {
        if (!IsSelfTarget && activateCardParams.TargetCharacterIds.Count() != TargetCharactersCount)
        {
            throw new ArgumentException("Invalid character count");
        }

        return new RerollCharacteristicActionCommand(
            CharacteristicType,
            activateCardParams.TargetCharacterIds,
            CharacteristicId,
            IsSelfTarget
        );
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return CharacteristicType;
        yield return CharacteristicId ?? Guid.Empty;
    }

    public class RerollCharacteristicActionCommand : CardActionCommand
    {
        public CharacteristicType CharacteristicType { get; }
        public IEnumerable<Guid> TargetCharactersIds { get; }
        public Guid? CharacteristicId { get; }
        public bool IsSelfTarget { get; }

        public RerollCharacteristicActionCommand(
            CharacteristicType characteristicType,
            IEnumerable<Guid> targetCharactersIds,
            Guid? characteristicId,
            bool isSelfTarget
        )
        {
            CharacteristicType = characteristicType;
            TargetCharactersIds = targetCharactersIds;
            CharacteristicId = characteristicId;
            IsSelfTarget = isSelfTarget;
        }
    }
}
