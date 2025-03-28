using Bunker.Domain.Shared.CardActionCommands;

namespace Bunker.Game.Domain.AggregateModels.Characters.Cards.CardActions;

public partial class AddCharacteristic : CardAction
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

    public override CardActionCommand CreateActionCommand(ActivateCardParams activateCardParams, Guid gameSessionId)
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
}
