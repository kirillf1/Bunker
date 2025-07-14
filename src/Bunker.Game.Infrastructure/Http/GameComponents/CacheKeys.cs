namespace Bunker.Game.Infrastructure.Http.GameComponents;

public static class CacheKeys
{
    // Character Components
    public const string CharacterAdditionalInformation = "character-components:additional-information";
    public const string CharacterCards = "character-components:cards";
    public const string CharacterHealth = "character-components:health";
    public const string CharacterHobbies = "character-components:hobbies";
    public const string CharacterItems = "character-components:items";
    public const string CharacterPhobias = "character-components:phobias";
    public const string CharacterProfessions = "character-components:professions";
    public const string CharacterTraits = "character-components:traits";

    // Bunker Components
    public const string BunkerDescriptions = "bunker-components:descriptions";
    public const string BunkerItems = "bunker-components:items";
    public const string BunkerEnvironments = "bunker-components:environments";
    public const string BunkerRooms = "bunker-components:rooms";

    // Catastrophe Components
    public const string CatastropheDescriptions = "catastrophe-components:descriptions";

    /// <summary>
    /// Генерирует ключ для отдельного элемента на основе базового ключа и ID
    /// </summary>
    /// <param name="baseKey">Базовый ключ (например, CharacterCards)</param>
    /// <param name="id">ID элемента</param>
    /// <returns>Полный ключ для элемента</returns>
    public static string GetItemKey(string baseKey, Guid id) => $"{baseKey}:{id}";
}
