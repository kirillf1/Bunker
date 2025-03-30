using Bunker.Domain.Shared.Cards.CardActionCommands;
using Bunker.Game.Domain.AggregateModels.Characters.Cards;
using Bunker.Game.Domain.AggregateModels.Characters.Characteristics;
using Bunker.Game.Domain.AggregateModels.Characters.Extensions;

namespace Bunker.Game.Domain.AggregateModels.Characters.CharacterCardCommandHandlers;

public class RemoveCharacteristicActionCommandHandler : ICardActionCommandHandler<RemoveCharacteristicActionCommand>
{
    private readonly ICharacterRepository _characterRepository;

    public RemoveCharacteristicActionCommandHandler(ICharacterRepository characterRepository)
    {
        _characterRepository = characterRepository;
    }

    public async Task Handle(RemoveCharacteristicActionCommand command)
    {
        var characteristicType = command.CharacteristicType.ToType();

        var characters = new List<Character>();

        foreach (var characterId in command.TargetCharactersIds)
        {
            var character = await _characterRepository.GetCharacter(characterId);

            if (character is not null)
            {
                characters.Add(character);
            }
        }

        if (characters.Count == 0)
        {
            throw new InvalidGameOperationException("No available target characters for use card");
        }

        foreach (var character in characters)
        {
            switch (characteristicType)
            {
                case Type t when t == typeof(Item):
                    character.RemoveItem(character.Items.First());
                    break;
                case Type t when t == typeof(Trait):
                    character.RemoveTrait(character.Traits.First());
                    break;
                case Type t when t == typeof(Card):
                    character.RemoveCard(character.Cards.First());
                    break;
                default:
                    throw new ArgumentException($"Unknown characteristic type: {characteristicType.Name}");
            }

            await _characterRepository.Update(character);
        }
    }
}
