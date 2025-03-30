using Bunker.Domain.Shared.Cards.CardActionCommands;
using Bunker.Game.Domain.AggregateModels.Characters.Cards;
using Bunker.Game.Domain.AggregateModels.Characters.Characteristics;
using Bunker.Game.Domain.AggregateModels.Characters.Extensions;

namespace Bunker.Game.Domain.AggregateModels.Characters.CharacterCardCommandHandlers;

public class AddCharacteristicActionCommandHandler : ICardActionCommandHandler<AddCharacteristicActionCommand>
{
    private readonly ICharacteristicGenerator _characteristicGenerator;
    private readonly ICharacterRepository _characterRepository;

    public AddCharacteristicActionCommandHandler(
        ICharacteristicGenerator characteristicGenerator,
        ICharacterRepository characterRepository
    )
    {
        _characteristicGenerator = characteristicGenerator;
        _characterRepository = characterRepository;
    }

    public async Task Handle(AddCharacteristicActionCommand command)
    {
        var charactersForReroll = new List<Character>();

        foreach (var characterId in command.TargetCharactersIds)
        {
            var character = await _characterRepository.GetCharacter(characterId);

            if (character is null || character.IsKicked)
                continue;

            charactersForReroll.Add(character);
        }

        if (charactersForReroll.Count == 0)
        {
            throw new InvalidGameOperationException("No available target characters for use card");
        }

        if (command.CharacteristicId is not null)
        {
            await AddCharacteristicByRequiredCharacteristic(
                command.CharacteristicType,
                command.CharacteristicId.Value,
                charactersForReroll
            );
        }
        else
        {
            await AddCharacteristicOnRandomCharacteristic(command.CharacteristicType, charactersForReroll);
        }
    }

    private async Task AddCharacteristicOnRandomCharacteristic(
        CharacteristicType characteristicType,
        IEnumerable<Character> characters
    )
    {
        var type = characteristicType.ToType();

        foreach (var character in characters)
        {
            var characteristic = await _characteristicGenerator.GenerateCharacteristic(type);

            AddCharacteristic(character, characteristic);
        }
    }

    private async Task AddCharacteristicByRequiredCharacteristic(
        CharacteristicType characteristicType,
        Guid requiredCharacteristicId,
        IEnumerable<Character> characters
    )
    {
        var type = characteristicType.ToType();

        var characteristic =
            await _characteristicGenerator.GetCharacteristic(requiredCharacteristicId, type)
            ?? throw new InvalidGameOperationException("Characteristic not found in this card");

        foreach (var character in characters)
        {
            AddCharacteristic(character, characteristic);

            await _characterRepository.Update(character);
        }
    }

    private static void AddCharacteristic(Character character, ICharacteristic characteristic)
    {
        switch (characteristic)
        {
            case Type t when t == typeof(Item):
                character.AddItem((Item)characteristic);
                break;
            case Type t when t == typeof(Trait):
                character.AddTrait((Trait)characteristic);
                break;
            case Type t when t == typeof(Card):
                character.AddCard((Card)characteristic);
                break;
            default:
                throw new InvalidGameOperationException($"Selected invalid characteristic for add");
        }
    }
}
