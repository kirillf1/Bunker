using System.Collections.Concurrent;
using System.Diagnostics;
using System.Text;
using System.Text.Json;
using Bunker.MessageBus.Abstractions;
using Confluent.Kafka;
using Confluent.Kafka.Extensions.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Bunker.MessageBus.Kafka.Consumers;

internal class KafkaEventConsumersManager : IDisposable
{
    private readonly Dictionary<string, ConsumerSettings> _consumersSettings;
    private readonly KafkaConnectionSettings _kafkaConnectionSettings;
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly EventBusSubscriptionInfo _subscriptionInfo;
    private readonly ILogger<KafkaEventConsumersManager> _logger;
    private readonly KafkaMetrics _kafkaMetrics;
    private readonly List<IConsumer<string, string>> _consumers;
    private readonly Lazy<IProducer<string, string>> _deadMessageProducer;
    private readonly ConcurrentDictionary<string, PartitionCommitManager> _partitionCommitManagers = new();
    private bool _disposed = false;

    public KafkaEventConsumersManager(
        KafkaConnectionSettings kafkaConnection,
        IServiceScopeFactory scopeFactory,
        IOptions<EventBusSubscriptionInfo> subscriptionOptions,
        ILogger<KafkaEventConsumersManager> logger,
        KafkaMetrics kafkaMetrics
    )
    {
        _kafkaConnectionSettings = kafkaConnection;
        _serviceScopeFactory = scopeFactory;
        _subscriptionInfo = subscriptionOptions.Value;
        _logger = logger;
        _kafkaMetrics = kafkaMetrics;
        _consumersSettings = new Dictionary<string, ConsumerSettings>();
        _consumers = [];
        _deadMessageProducer = new Lazy<IProducer<string, string>>(CreateDeadMessageProducer());
    }

    ~KafkaEventConsumersManager()
    {
        Dispose(false);
    }

    public void AddConsumer(string topic, ConsumerSettings consumerSettings)
    {
        _consumersSettings.Add(topic, consumerSettings);
    }

    public async Task StartConsumeEvents(CancellationToken cancellationToken)
    {
        var consumerTasks = new List<Task>();

        foreach (var consumerSetting in _consumersSettings)
        {
            consumerTasks.Add(
                Task.Factory.StartNew(
                    () => Consume(cancellationToken, consumerSetting.Value, consumerSetting.Key),
                    TaskCreationOptions.LongRunning
                )
            );
        }

        await Task.WhenAll(consumerTasks);
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        lock (_kafkaMetrics)
        {
            if (_disposed)
                return;

            if (disposing)
            {
                foreach (var consumer in _consumers.ToArray())
                {
                    try
                    {
                        consumer.Close();
                        consumer.Dispose();
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Failed close consumer");
                    }
                }

                if (_deadMessageProducer.IsValueCreated)
                    _deadMessageProducer.Value.Dispose();

                foreach (var commitManager in _partitionCommitManagers.Values)
                {
                    commitManager.Clear();
                }
                _partitionCommitManagers.Clear();
            }

            _disposed = true;
        }
    }

    private async Task Consume(CancellationToken cancellationToken, ConsumerSettings consumerSettings, string topic)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            try
            {
                await ConsumeInternal(cancellationToken, consumerSettings, topic);
                break;
            }
            catch (OperationCanceledException cancelledException)
            {
                _logger.LogWarning(cancelledException, "Consumer working cancelled");
                break;
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Can't consume message, reconnecting consumer");

                await Task.Delay(KafkaDefaults.ReconnectDelayMs, cancellationToken);
            }
        }
    }

    private async Task ConsumeInternal(
        CancellationToken cancellationToken,
        ConsumerSettings consumerSettings,
        string topic
    )
    {
        var consumerConfig = new ConsumerConfig
        {
            BootstrapServers = _kafkaConnectionSettings.BootstrapServers,
            GroupId = consumerSettings.GroupId,
            SaslPassword = _kafkaConnectionSettings.SaslPassword,
            SaslUsername = _kafkaConnectionSettings.SaslUsername,
            AutoOffsetReset = consumerSettings.AutoOffsetReset,
            EnableAutoCommit = false,
            AllowAutoCreateTopics = false,
            FetchWaitMaxMs = KafkaDefaults.FetchWaitMaxMs,
            Acks = Acks.Leader,
        };

        var consumer = new ConsumerBuilder<string, string>(consumerConfig)
            .SetKeyDeserializer(Deserializers.Utf8)
            .SetErrorHandler(
                (_, errorEvent) =>
                    _logger.LogError("Failed consume: {Reason}, topic: {Topic}", errorEvent.Reason, topic)
            )
            .SetLogHandler(
                (_, logEvent) =>
                {
                    if (logEvent.Level > SyslogLevel.Warning)
                        _logger.LogError(logEvent.Message);
                }
            )
            .SetPartitionsAssignedHandler(
                (consumerInstance, partitions) =>
                {
                    _logger.LogInformation(
                        "Partitions assigned: {Partitions}",
                        string.Join(", ", partitions.Select(p => $"{p.Topic}:{p.Partition}"))
                    );

                    foreach (var partition in partitions)
                    {
                        var partitionKey = $"{partition.Topic}:{partition.Partition}";

                        using var scope = _serviceScopeFactory.CreateScope();
                        var logger = scope.ServiceProvider.GetRequiredService<ILogger<PartitionCommitManager>>();

                        var commitManager = new PartitionCommitManager(partitionKey, logger);

                        _partitionCommitManagers[partitionKey] = commitManager;

                        // Получаем текущий committed offset для partition'а
                        try
                        {
                            var committed = consumerInstance.Committed(
                                [partition],
                                TimeSpan.FromSeconds(KafkaDefaults.CommittedOffsetTimeoutSeconds)
                            );
                            var committedOffset = committed.FirstOrDefault()?.Offset ?? Offset.Unset;

                            if (committedOffset == Offset.Unset)
                            {
                                var initialOffset =
                                    consumerSettings.AutoOffsetReset == AutoOffsetReset.Earliest
                                        ? KafkaDefaults.InitialOffset
                                        : long.MaxValue;

                                commitManager.InitializeLastCommittedOffset(initialOffset);
                            }
                            else
                            {
                                commitManager.InitializeLastCommittedOffset(committedOffset.Value - 1);
                            }
                        }
                        catch (Exception exception)
                        {
                            _logger.LogWarning(
                                exception,
                                "Failed to get committed offset for {PartitionKey}, using {InitialOffset}",
                                partitionKey,
                                KafkaDefaults.InitialOffset
                            );
                            commitManager.InitializeLastCommittedOffset(KafkaDefaults.InitialOffset);
                        }
                    }
                }
            )
            .SetPartitionsRevokedHandler(
                (consumerInstance, partitions) =>
                {
                    _logger.LogInformation(
                        "Partitions revoked: {Partitions}",
                        string.Join(", ", partitions.Select(p => $"{p.Topic}:{p.Partition}"))
                    );

                    foreach (var partition in partitions)
                    {
                        var partitionKey = $"{partition.Topic}:{partition.Partition}";
                        if (_partitionCommitManagers.TryRemove(partitionKey, out var commitManager))
                        {
                            commitManager.Clear();
                        }
                    }
                }
            )
            .Build();

        _consumers.Add(consumer);

        cancellationToken.ThrowIfCancellationRequested();

        try
        {
            consumer.Subscribe(topic);

            using var consumingStrategy = consumerSettings.ConsumeStrategy;

            await consumingStrategy.InitializeEventHandlers(
                _subscriptionInfo,
                (consumeResult, eventType) =>
                    HandleMessageWithDlqWrapper(consumeResult, eventType, consumer, cancellationToken),
                (consumeResult) => CommitMessage(consumeResult, consumer, cancellationToken)
            );

            _logger.LogInformation("Consumer subscribed to topics: {Topic}", topic);

            while (!cancellationToken.IsCancellationRequested)
            {
                var consumeResult = consumer.Consume(cancellationToken);

                if (consumeResult is null)
                    continue;

                var headers = consumeResult.Message.Headers;

                if (headers is null || !headers.TryGetLastBytes("message-type", out var messageTypeBytes))
                {
                    _logger.LogWarning(
                        "Unknown event, no message-type header. Committing message: {Topic}:{Partition}:{Offset}",
                        consumeResult.Topic,
                        consumeResult.Partition,
                        consumeResult.Offset
                    );

                    await CommitMessage(consumeResult, consumer, cancellationToken);

                    continue;
                }

                var messageType = Encoding.UTF8.GetString(headers.GetLastBytes("message-type"));
                if (!_subscriptionInfo.EventTypes.TryGetValue(messageType, out var eventType))
                {
                    _logger.LogWarning(
                        "Unable to resolve message type for message name {MessageName}. Committing message: {Topic}:{Partition}:{Offset}",
                        messageType,
                        consumeResult.Topic,
                        consumeResult.Partition,
                        consumeResult.Offset
                    );

                    await CommitMessage(consumeResult, consumer, cancellationToken);

                    continue;
                }

                await consumingStrategy.HandleEvent(consumeResult, eventType, cancellationToken);
            }
        }
        finally
        {
            consumer.Close();
            _consumers.Remove(consumer);
        }
    }

    private async Task CommitMessage(
        ConsumeResult<string, string> consumeResult,
        IConsumer<string, string> consumer,
        CancellationToken cancellationToken
    )
    {
        var partitionKey = $"{consumeResult.Topic}:{consumeResult.Partition}";

        if (_partitionCommitManagers.TryGetValue(partitionKey, out var commitManager))
        {
            await commitManager.TryCommitInOrder(consumeResult, consumer, cancellationToken);
        }
        else
        {
            _logger.LogWarning("No commit manager found for partition {PartitionKey}", partitionKey);
        }
    }

    private async Task HandleMessageWithDlqWrapper(
        ConsumeResult<string, string> consumeResult,
        Type eventType,
        IConsumer<string, string> consumer,
        CancellationToken cancellationToken
    )
    {
        try
        {
            await HandleMessage(consumeResult, eventType);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Message processing failed, sending to DLQ and committing");

            await HandleErrorMessage(consumeResult, eventType, exception, CancellationToken.None);

            await CommitMessage(consumeResult, consumer, cancellationToken);
        }
    }

    private async Task HandleMessage(ConsumeResult<string, string> consumeResult, Type eventType)
    {
        var activity = ConsumeActivityExtensions.StartConsumeActivity(consumeResult);
        try
        {
            var integrationEvent =
                JsonSerializer.Deserialize(
                    consumeResult.Message.Value,
                    eventType,
                    _subscriptionInfo.JsonSerializerOptions
                ) as IntegrationEvent;

            using var scope = _serviceScopeFactory.CreateAsyncScope();

            foreach (var handler in scope.ServiceProvider.GetKeyedServices<IIntegrationEventHandler>(eventType))
            {
                await handler.Handle(integrationEvent!);
            }

            _kafkaMetrics.AddMessageProcessed();
        }
        finally
        {
            activity?.Stop();
        }
    }

    private IProducer<string, string> CreateDeadMessageProducer()
    {
        var producerConfig = new ProducerConfig
        {
            BootstrapServers = _kafkaConnectionSettings.BootstrapServers,
            SaslPassword = _kafkaConnectionSettings.SaslPassword,
            SaslUsername = _kafkaConnectionSettings.SaslUsername,
            RequestTimeoutMs = KafkaDefaults.ProducerRequestTimeoutMs,
            MessageSendMaxRetries = KafkaDefaults.MessageSendMaxRetries,
            MessageTimeoutMs = KafkaDefaults.MessageTimeoutMs,
            AllowAutoCreateTopics = true,
        };

        return new ProducerBuilder<string, string>(producerConfig).BuildWithInstrumentation();
    }

    private async Task HandleErrorMessage(
        ConsumeResult<string, string> consumeResult,
        Type messageType,
        Exception exception,
        CancellationToken cancellationToken
    )
    {
        _logger.LogError(
            exception,
            "Can't handle message: {MessageType}, sending to dead message topic",
            messageType.Name
        );

        _kafkaMetrics.AddMessageProcessedWithError();

        var headers = consumeResult.Message.Headers;

        headers ??= new Headers();

        var activity = Activity.Current;

        var traceId = activity?.TraceId.ToString();
        if (traceId is not null)
            headers.Add("error-trace-id", Encoding.UTF8.GetBytes(traceId));

        var spanId = activity?.SpanId.ToString();
        if (spanId is not null)
            headers.Add("error-span-id", Encoding.UTF8.GetBytes(spanId));

        var message = new Message<string, string>
        {
            Headers = headers,
            Key = consumeResult.Message.Key,
            Timestamp = consumeResult.Message.Timestamp,
            Value = consumeResult.Message.Value,
        };

        await _deadMessageProducer.Value.ProduceAsync("dead-message-topic", message, cancellationToken);
    }
}
