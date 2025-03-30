using Bunker.Domain.Shared.Cards.CardActionCommands;
using Bunker.Game.Domain.AggregateModels.Characters.Cards;
using Bunker.Game.Domain.AggregateModels.Characters.Characteristics;
using Bunker.Game.Domain.AggregateModels.Characters.Extensions;

namespace Bunker.Game.Domain.AggregateModels.Characters.CharacterCardCommandHandlers;

public class RerollCharacteristicActionCommandHandler : ICardActionCommandHandler<RerollCharacteristicActionCommand>
{
    private readonly ICharacteristicGenerator _characteristicGenerator;
    private readonly ICharacterRepository _characterRepository;

    public RerollCharacteristicActionCommandHandler(
        ICharacteristicGenerator characteristicGenerator,
        ICharacterRepository characterRepository
    )
    {
        _characteristicGenerator = characteristicGenerator;
        _characterRepository = characterRepository;
    }

    public async Task Handle(RerollCharacteristicActionCommand command)
    {
        if (command.IsSelfTarget && command.TargetCharactersIds.Count() != 1)
        {
            throw new ArgumentException(
                $"Invalid characters count for use card. Expected: 1, Added: {command.TargetCharactersIds.Count()}"
            );
        }

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
            await RerollCharacteristicByRequiredCharacteristic(
                command.CharacteristicType,
                command.CharacteristicId.Value,
                charactersForReroll
            );
        }
        else
        {
            await RerollCharacteristicOnRandomCharacteristic(command.CharacteristicType, charactersForReroll);
        }
    }

    private async Task RerollCharacteristicOnRandomCharacteristic(
        CharacteristicType characteristicType,
        IEnumerable<Character> characters
    )
    {
        var type = characteristicType.ToType();

        foreach (var character in characters)
        {
            var characteristic = await _characteristicGenerator.GenerateCharacteristic(type);

            ChangeCharacteristic(character, characteristic);

            await _characterRepository.Update(character);
        }
    }

    private async Task RerollCharacteristicByRequiredCharacteristic(
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
            ChangeCharacteristic(character, characteristic);
        }
    }

    private static void ChangeCharacteristic(Character character, ICharacteristic characteristic)
    {
        switch (characteristic)
        {
            case Type t when t == typeof(AdditionalInformation):
                character.UpdateAdditionalInformation((AdditionalInformation)characteristic);
                break;
            case Type t when t == typeof(Health):
                character.UpdateHealth((Health)characteristic);
                break;
            case Type t when t == typeof(Hobby):
                character.UpdateHobby((Hobby)characteristic);
                break;
            case Type t when t == typeof(Phobia):
                character.UpdatePhobia((Phobia)characteristic);
                break;
            case Type t when t == typeof(Profession):
                character.UpdateProfession((Profession)characteristic);
                break;
            case Type t when t == typeof(Item):
                character.ReplaceItem(character.Items.First(), (Item)characteristic);
                break;
            case Type t when t == typeof(Trait):
                character.ReplaceTrait(character.Traits.First(), (Trait)characteristic);
                break;
            case Type t when t == typeof(Card):
                character.ReplaceCard(character.Cards.First(), (Card)characteristic);
                break;
            default:
                throw new ArgumentException($"Unknown characteristic type: {characteristic.GetType().Name}");
        }
    }
}
