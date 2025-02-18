using Bunker.Game.Domain.AggregateModels.Characters.Cards;
using Bunker.Game.Domain.AggregateModels.Characters.Characteristics;

namespace Bunker.Game.Domain.AggregateModels.Characters.Extensions;

internal static class CharacteristicTypeExtensions
{
    public static Type ToType(this CharacteristicType characteristicType)
    {
        return characteristicType switch
        {
            CharacteristicType.Phobia => typeof(Phobia),
            CharacteristicType.Hobby => typeof(Hobby),
            CharacteristicType.AdditionalInformation => typeof(AdditionalInformation),
            CharacteristicType.Health => typeof(Health),
            CharacteristicType.CharacterItem => typeof(Item),
            CharacteristicType.Profession => typeof(Profession),
            CharacteristicType.Trait => typeof(Trait),
            CharacteristicType.Card => typeof(Card),
            _ => throw new ArgumentException($"Unknown characteristic type: {characteristicType}"),
        };
    }

    public static void UpdateCharacteristic(
        this Character character,
        ICharacteristic characteristic,
        ICharacteristic? charact
    )
    {
        var characteristicType = characteristic.GetType();

        switch (characteristicType)
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
                character.AddItem((Item)characteristic);
                break;
            case Type t when t == typeof(Trait):
                character.AddTrait((Trait)characteristic);
                break;
            case Type t when t == typeof(Card):
                character.AddCard((Card)characteristic);
                break;
            default:
                throw new ArgumentException($"Unknown characteristic type: {characteristicType.Name}");
        }
    }
}
