using Bunker.MessageBus.Abstractions;
using Confluent.Kafka;

namespace Bunker.MessageBus.Kafka.Consumers.ConsumeStrategies;

public interface IConsumeStrategy : IDisposable
{
    Task InitializeEventHandlers(
        EventBusSubscriptionInfo eventBusSubscriptionInfo,
        Func<ConsumeResult<string, string>, Type, Task> handler,
        Func<ConsumeResult<string, string>, Task>? onProcessedCallback = null
    );
    Task HandleEvent(ConsumeResult<string, string> consumeResult, Type eventType, CancellationToken cancellationToken);
}
