using Bunker.Domain.Shared.Cards.CardActionCommands;

namespace Bunker.Game.Domain.AggregateModels.Characters.Cards.CardActions;

public partial class AddCharacteristicCardAction : CardAction
{
    public CharacteristicType CharacteristicType { get; }
    public Guid? CharacteristicId { get; }
    public int TargetCharactersCount { get; }

    public AddCharacteristicCardAction(CharacteristicType characteristicType, Guid? characteristicId, int targetCharactersCount)
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
