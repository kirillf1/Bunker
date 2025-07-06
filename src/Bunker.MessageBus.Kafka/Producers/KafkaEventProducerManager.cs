using System.Text;
using System.Text.Json;
using Bunker.MessageBus.Abstractions;
using Confluent.Kafka;
using Confluent.Kafka.Extensions.Diagnostics;
using Microsoft.Extensions.Logging;

namespace Bunker.MessageBus.Kafka.Producers;

internal class KafkaEventProducerManager : IDisposable
{
    private readonly KafkaConnectionSettings _kafkaConnectionSettings;
    private readonly ILogger<KafkaEventProducerManager> _logger;
    private readonly KafkaMetrics _kafkaMetrics;
    private readonly Dictionary<Type, BindEventToProducerSettings> _eventSendParams;
    private readonly Lazy<IProducer<string, string>> _producer;
    private readonly JsonSerializerOptions _jsonSerializerOptions;
    private bool _disposed = false;

    public KafkaEventProducerManager(
        KafkaConnectionSettings kafkaConnectionSettings,
        ILogger<KafkaEventProducerManager> logger,
        KafkaMetrics kafkaMetrics
    )
    {
        _kafkaConnectionSettings = kafkaConnectionSettings;
        _logger = logger;
        _kafkaMetrics = kafkaMetrics;
        _eventSendParams = new Dictionary<Type, BindEventToProducerSettings>();
        _producer = new Lazy<IProducer<string, string>>(CreateProducer, true);
        _jsonSerializerOptions = new JsonSerializerOptions { WriteIndented = true };
    }

    ~KafkaEventProducerManager()
    {
        Dispose(false);
    }

    public void AddEventSendParams(Type type, BindEventToProducerSettings sendParams)
    {
        _eventSendParams[type] = sendParams;
    }

    public async Task ProduceEvent(IntegrationEvent integrationEvent)
    {
        try
        {
            _logger.LogInformation("Sending event: {EventName} to kafka", integrationEvent.GetType().Name);

            var message = CreateMessageForProducer(integrationEvent, out var topic);
            await _producer.Value.ProduceAsync(topic, message);

            _kafkaMetrics.AddMessagePublished();
            _logger.LogInformation("Event sent: {EventName} to kafka", integrationEvent.GetType().Name);
        }
        catch (Exception exception)
        {
            _kafkaMetrics.AddMessagePublishedWithError();
            _logger.LogError(exception, "Failed send event: {EventName} to kafka", integrationEvent.GetType().Name);
            throw;
        }
    }

    /// <summary>
    /// Effective sending if you have many events
    /// </summary>
    /// <param name="integrationEvents"></param>
    /// <returns></returns>
    public async Task ProduceBatchEvents(IEnumerable<IntegrationEvent> integrationEvents)
    {
        try
        {
            _producer.Value.BeginTransaction();
            var tasks = new List<Task>();

            foreach (var integrationEvent in integrationEvents)
            {
                tasks.Add(ProduceEvent(integrationEvent));
            }

            await Task.WhenAll(tasks);
            _producer.Value.CommitTransaction();
        }
        catch (Exception)
        {
            _producer.Value.AbortTransaction();
            throw;
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (_disposed)
            return;

        if (_producer.IsValueCreated)
            _producer.Value.Dispose();

        _disposed = true;
    }

    private Message<string, string> CreateMessageForProducer(IntegrationEvent integrationEvent, out string topic)
    {
        var eventType = integrationEvent.GetType();
        if (!_eventSendParams.TryGetValue(eventType, out var sendParams) || sendParams.TargetTopic is null)
        {
            _logger.LogError(
                "Unknown event for kafka producer, eventType: {EventType}. Add this event with topic name to kafka configuration",
                eventType.Name
            );
            throw new InvalidOperationException("Unknown event for produce");
        }

        var eventName = sendParams.EventName ?? integrationEvent.GetType().Name;
        var key = sendParams.PartitionKeyFunction is null ? null : sendParams.PartitionKeyFunction(integrationEvent);
        var messageJson = JsonSerializer.Serialize(integrationEvent, eventType, _jsonSerializerOptions);
#pragma warning disable CS8601 // Возможно, назначение-ссылка, допускающее значение NULL.
        var message = new Message<string, string>
        {
            Key = key,
            Value = messageJson,
            Headers = [],
        };
#pragma warning restore CS8601 // Возможно, назначение-ссылка, допускающее значение NULL.
        message.Headers.Add("message-type", Encoding.UTF8.GetBytes(eventName));
        topic = sendParams.TargetTopic;
        return message;
    }

    private IProducer<string, string> CreateProducer()
    {
        var producerConfig = new ProducerConfig
        {
            BootstrapServers = _kafkaConnectionSettings.BootstrapServers,
            SaslPassword = _kafkaConnectionSettings.SaslPassword,
            SaslUsername = _kafkaConnectionSettings.SaslUsername,
            RequestTimeoutMs = KafkaDefaults.ProducerRequestTimeoutMs,
            MessageSendMaxRetries = KafkaDefaults.MessageSendMaxRetries,
            MessageTimeoutMs = KafkaDefaults.MessageTimeoutMs,
            AllowAutoCreateTopics = false,
            Acks = Acks.Leader,
        };

        return new ProducerBuilder<string, string>(producerConfig).BuildWithInstrumentation();
    }
}
