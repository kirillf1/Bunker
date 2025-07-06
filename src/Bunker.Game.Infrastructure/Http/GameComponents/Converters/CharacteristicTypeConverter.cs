namespace Bunker.Game.Infrastructure.Http.GameComponents.Converters;

public static class CharacteristicTypeConverter
{
    public static Bunker.Domain.Shared.GameComponents.CharacteristicType FromDto(this Contracts.CharacteristicType dto)
    {
        string enumName = Enum.GetName(dto)!;
        return Enum.Parse<Bunker.Domain.Shared.GameComponents.CharacteristicType>(enumName);
    }
}
