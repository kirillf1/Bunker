using static Bunker.Game.Domain.AggregateModels.Characters.Cards.CardActions.SpyCharacteristic;

namespace Bunker.Game.Domain.AggregateModels.Characters.Cards.CardActions;

public class RemoveCharacteristic : CardAction
{
    public CharacteristicType CharacteristicType { get; }
    public int TargetCharactersCount { get; }

    public RemoveCharacteristic(
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

    public class RemoveCharacteristicCommand : CardActionCommand
    {
        public CharacteristicType CharacteristicType { get; }
        public IEnumerable<Guid> TargetCharactersIds { get; }

        public RemoveCharacteristicCommand(CharacteristicType characteristicType, IEnumerable<Guid> targetCharactersIds)
        {
            CharacteristicType = characteristicType;
            TargetCharactersIds = targetCharactersIds;
        }
    }
}
