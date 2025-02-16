using Bunker.Domain.Shared.CardActionCommands;
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

        return new RemoveCharacteristicActionCommand(CharacteristicType, activateCardParams.TargetCharacterIds);
    }

    public override CardActionRequirements GetCurrentCardActionRequirements()
    {
        return new CardActionRequirements(ActivateCardTargetType.Character, TargetCharactersCount);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return CharacteristicType;
        yield return TargetCharactersCount;
    }
}
