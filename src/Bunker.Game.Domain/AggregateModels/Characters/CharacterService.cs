using Bunker.Game.Domain.AggregateModels.Characters.Cards;

namespace Bunker.Game.Domain.AggregateModels.Characters;

public class CharacterService
{
    private readonly ICardCommandExecutor _cardCommandExecutor;

    public CharacterService(ICardCommandExecutor cardCommandExecutor)
    {
        _cardCommandExecutor = cardCommandExecutor;
    }

    public async Task ActivateCharacterCard(Character character, Guid cardId, ActivateCardParams cardParams)
    {
        var cardCommand = character.UseCard(cardId, cardParams);

        await _cardCommandExecutor.ExecuteCardAction(cardCommand);
    }
}
