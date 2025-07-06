using Bunker.MessageBus.Abstractions;
using Bunker.MessageBus.Kafka.Consumers;
using Bunker.MessageBus.Kafka.Producers;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Bunker.MessageBus.Kafka;

public class KafkaMessageBus : IMessageBus, IHostedService
{
    private bool _stopped;
    private readonly ILogger<KafkaMessageBus> _logger;
    private readonly KafkaEventProducerManager _kafkaEventProducer;
    private readonly KafkaEventConsumersManager _eventConsumers;
    private readonly CancellationTokenSource _cts;

    internal KafkaMessageBus(
        ILogger<KafkaMessageBus> logger,
        KafkaEventProducerManager kafkaEventProducer,
        KafkaEventConsumersManager eventConsumers
    )
    {
        _logger = logger;
        _kafkaEventProducer = kafkaEventProducer;
        _eventConsumers = eventConsumers;
        _cts = new CancellationTokenSource();
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting consume events");

        _ = Task.Factory.StartNew(
            async () => await _eventConsumers.StartConsumeEvents(_cts.Token),
            TaskCreationOptions.LongRunning
        );

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        lock (_eventConsumers)
        {
            if (_stopped)
                return Task.CompletedTask;

            _stopped = true;

            _cts.Cancel();
            _kafkaEventProducer.Dispose();
            _eventConsumers.Dispose();
            _cts.Dispose();

            return Task.CompletedTask;
        }
    }

    public async Task PublishAsync(IntegrationEvent @event)
    {
        await _kafkaEventProducer.ProduceEvent(@event);
    }

    public async Task PublishAsync(IEnumerable<IntegrationEvent> events)
    {
        await _kafkaEventProducer.ProduceBatchEvents(events);
    }
}
