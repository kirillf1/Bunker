using Bunker.Domain.Shared.Cards.CardActionCommands;

namespace Bunker.Game.Domain.AggregateModels.Characters.Cards.CardActions;

public class RerollCharacteristicCardAction : CardAction
{
    public CharacteristicType CharacteristicType { get; }
    public bool IsSelfTarget { get; }
    public Guid? CharacteristicId { get; }
    public int TargetCharactersCount { get; }

    public RerollCharacteristicCardAction(
        CharacteristicType characteristicType,
        bool isSelfTarget,
        Guid? characteristicId,
        int targetCharactersCount
    )
    {
        CharacteristicType = characteristicType;
        IsSelfTarget = isSelfTarget;
        CharacteristicId = characteristicId;
        TargetCharactersCount = targetCharactersCount;
    }

    public override CardActionRequirements GetCurrentCardActionRequirements()
    {
        return new CardActionRequirements(ActivateCardTargetType.Character, TargetCharactersCount);
    }

    public override CardActionCommand CreateActionCommand(ActivateCardParams activateCardParams, Guid gameSessionId)
    {
        if (activateCardParams.TargetCharacterIds.Count() != TargetCharactersCount)
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
}
