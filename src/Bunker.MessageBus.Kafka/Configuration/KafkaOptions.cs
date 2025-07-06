using System.ComponentModel.DataAnnotations;

namespace Bunker.MessageBus.Kafka.Configuration;

/// <summary>
/// Настройки подключения и конфигурации Kafka.
/// </summary>
public class KafkaOptions
{
    public const string Section = "Kafka";

    /// <summary>
    /// Список серверов Kafka в формате "host:port".
    /// </summary>
    [Required]
    public string Servers { get; set; } = string.Empty;

    /// <summary>
    /// Логин для аутентификации в Kafka.
    /// </summary>
    public string Login { get; set; } = string.Empty;

    /// <summary>
    /// Пароль для аутентификации в Kafka.
    /// </summary>
    public string Password { get; set; } = string.Empty;

    /// <summary>
    /// Название топика для запросов завершения игры.
    /// </summary>
    [Required]
    public string CreateGameResultRequestsTopicName { get; set; } = string.Empty;

    /// <summary>
    /// Название топика для результатов игры.
    /// </summary>
    [Required]
    public string CreateGameResultResponsesTopicName { get; set; } = string.Empty;
}
