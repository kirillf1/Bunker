using System.Diagnostics.Metrics;

namespace Bunker.MessageBus.Kafka;

[System.Diagnostics.CodeAnalysis.SuppressMessage(
    "Major Code Smell",
    "T0038:Internal Styling Rule T0038",
    Justification = "<Ожидание>"
)]
public class KafkaMetrics
{
    public const string Name = "Kafka";
    private static Meter _meter = new(Name);
    private int _errorMessagesPublished = KafkaDefaults.MetricsInitialValue;
    private int _errorMessagesProcessed = KafkaDefaults.MetricsInitialValue;

    private Counter<int> MessageProcessedCounter { get; }
    private Counter<int> MessagePublishedCounter { get; }
    private ObservableCounter<int> ErrorPublishedCounter { get; }
    private ObservableCounter<int> ErrorProcessedMessageCounter { get; }

    public KafkaMetrics()
    {
        MessageProcessedCounter = _meter.CreateCounter<int>("kafka_messages_processed", "count");
        MessageProcessedCounter.Add(KafkaDefaults.MetricsInitialValue);
        MessagePublishedCounter = _meter.CreateCounter<int>("kafka_messages_published", "count");
        MessagePublishedCounter.Add(KafkaDefaults.MetricsInitialValue);

        ErrorPublishedCounter = _meter.CreateObservableCounter(
            "kafka_messages_published_with_error",
            () => _errorMessagesPublished,
            "count"
        );
        ErrorProcessedMessageCounter = _meter.CreateObservableCounter(
            "kafka_messages_processed_with_error",
            () => _errorMessagesProcessed,
            "count"
        );
    }

    public void AddMessageProcessed() => MessageProcessedCounter.Add(KafkaDefaults.MetricsIncrement);

    public void AddMessagePublished() => MessagePublishedCounter.Add(KafkaDefaults.MetricsIncrement);

    public void AddMessageProcessedWithError() => _errorMessagesProcessed += KafkaDefaults.MetricsIncrement;

    public void AddMessagePublishedWithError() => _errorMessagesPublished += KafkaDefaults.MetricsIncrement;
}
