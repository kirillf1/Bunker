namespace Bunker.Game.Infrastructure.Http.GameComponents;

public class GameComponentsClientOptions
{
    public const string Section = "GameComponents";

    public string Address { get; set; } = "";

    /// <summary>
    /// Время жизни кэша для HTTP клиентов в минутах. По умолчанию: 30 минут
    /// </summary>
    public int CacheExpirationTimeInMinutes { get; set; } = 30;

    /// <summary>
    /// Включено ли кэширование. По умолчанию: true
    /// </summary>
    public bool CacheEnabled { get; set; } = true;
}
