using Bunker.Domain.Shared.Cards.CardActionCommands;

namespace Bunker.Game.Domain.AggregateModels.Characters.Cards.CardActions;

public abstract class CardAction : ValueObject
{
    public abstract CardActionCommand CreateActionCommand(ActivateCardParams activateCardParams, Guid gameSessionId);

    public abstract CardActionRequirements GetCurrentCardActionRequirements();
}
