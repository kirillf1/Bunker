namespace Bunker.Game.Domain.AggregateModels.Characters.Cards.CardActions;

public abstract class CardAction : ValueObject
{
    public abstract CardActionCommand CreateActionCommand(ActivateCardParams activateCardParams);

    public abstract CardActionRequirements GetCurrentCardActionRequirements();

    public CardActionRequirements CardActionRequirements { get; protected set; }

    protected CardAction(CardActionRequirements cardActionRequirements)
    {
        CardActionRequirements = cardActionRequirements;
    }
}
