using Bunker.Domain.Shared.CardActionCommands;

namespace Bunker.Game.Domain.AggregateModels.Characters.Cards.CardActions;

public class RerollCharacteristicCardAction : CardAction
{
    public CharacteristicType CharacteristicType { get; }
    public bool IsSelfTarget { get; }
    public Guid? CharacteristicId { get; }
    public int TargetCharactersCount { get; }

    public RerollCharacteristicCardAction(
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

    public override CardActionRequirements GetCurrentCardActionRequirements()
    {
        return new CardActionRequirements(ActivateCardTargetType.None, 0);
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
        yield return CardActionRequirements;
        yield return CharacteristicType;
        yield return CharacteristicId ?? Guid.Empty;
    }
}
