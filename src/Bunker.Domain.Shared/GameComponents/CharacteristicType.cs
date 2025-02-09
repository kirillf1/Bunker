namespace Bunker.Domain.Shared.GameComponents;

[Flags]
public enum CharacteristicType
{
    Phobia = 1,
    Hobby = 2,
    AdditionalInformation = 4,
    Health = 8,
    CharacterItem = 16,
    Profession = 32,
    Trait = 64,
    Card = 128,
}
