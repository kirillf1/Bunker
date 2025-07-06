using Bunker.MessageBus.Kafka.Consumers.ConsumeStrategies;
using Confluent.Kafka;

namespace Bunker.MessageBus.Kafka.Consumers;

public class ConsumerSettings
{
    public IConsumeStrategy ConsumeStrategy { get; set; }
    public string GroupId { get; }
    public AutoOffsetReset AutoOffsetReset { get; }

    public ConsumerSettings(string groupId, AutoOffsetReset autoOffsetReset = AutoOffsetReset.Earliest)
    {
        GroupId = groupId;
        AutoOffsetReset = autoOffsetReset;
        ConsumeStrategy = new MultiThreadForEventStrategy();
    }

    public ConsumerSettings(
        string groupId,
        IConsumeStrategy consumeStrategy,
        AutoOffsetReset autoOffsetReset = AutoOffsetReset.Earliest
    )
    {
        GroupId = groupId;
        AutoOffsetReset = autoOffsetReset;
        ConsumeStrategy = consumeStrategy;
    }
}
