namespace Bunker.Game.Infrastructure.Application.Decorators;

/// <summary>
/// Константы для имен источников активности (ActivitySource)
/// </summary>
public static class ActivitySourceConstants
{
    /// <summary>
    /// Имя источника активности для команд
    /// </summary>
    public const string CommandsActivitySourceName = "Bunker.Application.Commands";
    
    /// <summary>
    /// Имя источника активности для запросов
    /// </summary>
    public const string QueriesActivitySourceName = "Bunker.Application.Queries";
} 