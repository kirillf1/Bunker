namespace Bunker.MessageBus.Kafka;

/// <summary>
/// Константы по умолчанию для конфигурации Kafka
/// </summary>
public static class KafkaDefaults
{
    /// <summary>
    /// Задержка перед повторным подключением consumer при сбое (миллисекунды)
    /// </summary>
    public const int ReconnectDelayMs = 2000;

    /// <summary>
    /// Максимальное время ожидания fetch запроса (миллисекунды)
    /// </summary>
    public const int FetchWaitMaxMs = 100;

    /// <summary>
    /// Таймаут для получения committed offset (секунды)
    /// </summary>
    public const int CommittedOffsetTimeoutSeconds = 10;

    /// <summary>
    /// Начальное значение offset для инициализации
    /// </summary>
    public const long InitialOffset = -1;

    /// <summary>
    /// Таймаут для producer запросов (миллисекунды)
    /// </summary>
    public const int ProducerRequestTimeoutMs = 2000;

    /// <summary>
    /// Максимальное количество повторных попыток отправки сообщения
    /// </summary>
    public const int MessageSendMaxRetries = 5;

    /// <summary>
    /// Таймаут для отправки сообщения (миллисекунды)
    /// </summary>
    public const int MessageTimeoutMs = 10000;

    /// <summary>
    /// Начальное значение для счетчиков метрик
    /// </summary>
    public const int MetricsInitialValue = 0;

    /// <summary>
    /// Инкремент для счетчиков метрик
    /// </summary>
    public const int MetricsIncrement = 1;

    /// <summary>
    /// Максимальное количество одновременных потоков обработки событий по умолчанию
    /// </summary>
    public const int DefaultMaxConcurrentThreadsCount = 1;

    /// <summary>
    /// Максимальный размер очереди для обработки событий по умолчанию
    /// </summary>
    public const int DefaultMaxQueueSize = 100;
} 