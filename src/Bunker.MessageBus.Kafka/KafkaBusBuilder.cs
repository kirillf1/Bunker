using System.Reflection;
using Bunker.MessageBus.Abstractions;
using Bunker.MessageBus.Kafka.Consumers;
using Bunker.MessageBus.Kafka.Producers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Bunker.MessageBus.Kafka;

public class KafkaBusBuilder
{
    private readonly KafkaEventConsumersManager _kafkaEventConsumerManager;
    private readonly IServiceProvider _serviceProvider;
    private readonly KafkaEventProducerManager _kafkaEventProducerManager;

    public KafkaBusBuilder(
        KafkaConnectionSettings kafkaConnectionSettings,
        IServiceProvider serviceProvider,
        IOptions<EventBusSubscriptionInfo> subscriptionOptions
    )
    {
        var kafkaMetrics = new KafkaMetrics();
        _serviceProvider = serviceProvider;
        var serviceScopeFactory = _serviceProvider.GetRequiredService<IServiceScopeFactory>();
        var consumerLogger = serviceProvider.GetRequiredService<ILogger<KafkaEventConsumersManager>>();

        _kafkaEventConsumerManager = new KafkaEventConsumersManager(
            kafkaConnectionSettings,
            serviceScopeFactory,
            subscriptionOptions,
            consumerLogger,
            kafkaMetrics
        );

        var eventProducerManagerLogger = serviceProvider.GetRequiredService<ILogger<KafkaEventProducerManager>>();
        _kafkaEventProducerManager = new KafkaEventProducerManager(
            kafkaConnectionSettings,
            eventProducerManagerLogger,
            kafkaMetrics
        );
    }

    public KafkaBusBuilder BindEventsToProducer(
        string topicName,
        Dictionary<Type, BindEventToProducerSettings> eventParams
    )
    {
        foreach (var eventParam in eventParams)
        {
            eventParam.Value.TargetTopic = topicName;
            _kafkaEventProducerManager.AddEventSendParams(eventParam.Key, eventParam.Value);
        }

        return this;
    }

    public KafkaBusBuilder BindEventsToProducerByAssembly(string topicName, Assembly assembly)
    {
        var events = assembly.GetTypes().Where(x => x.IsSubclassOf(typeof(IntegrationEvent)));

        foreach (var eventType in events)
        {
            _kafkaEventProducerManager.AddEventSendParams(
                eventType,
                new BindEventToProducerSettings { TargetTopic = topicName }
            );
        }

        return this;
    }

    public KafkaBusBuilder BindEventToProducer<T>(BindEventToProducerSettings eventParams)
        where T : IntegrationEvent
    {
        _kafkaEventProducerManager.AddEventSendParams(typeof(T), eventParams);

        return this;
    }

    public KafkaBusBuilder AddEventConsumer(string topic, ConsumerSettings consumerSettings)
    {
        _kafkaEventConsumerManager.AddConsumer(topic, consumerSettings);

        return this;
    }

    public KafkaMessageBus Build()
    {
        var logger = _serviceProvider.GetRequiredService<ILogger<KafkaMessageBus>>();

        return new KafkaMessageBus(logger, _kafkaEventProducerManager, _kafkaEventConsumerManager);
    }
}
