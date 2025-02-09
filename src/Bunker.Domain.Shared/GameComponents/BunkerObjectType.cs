namespace Bunker.Domain.Shared.GameComponents;

[Flags]
public enum BunkerObjectType
{
    BunkerRoom = 1,
    BunkerEnvironment = 2,
    BunkerItem = 4,
}
