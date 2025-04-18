using Bunker.Game.Infrastructure.Http.GameComponents.Contracts;

namespace Bunker.Game.Infrastructure.Http.GameComponents.Converters;

public static class BunkerObjectTypeConverter
{
    public static Bunker.Domain.Shared.GameComponents.BunkerObjectType FromDto(this BunkerObjectType dto)
    {
        string enumName = Enum.GetName(dto)!;
        return Enum.Parse<Bunker.Domain.Shared.GameComponents.BunkerObjectType>(enumName);
    }
}
