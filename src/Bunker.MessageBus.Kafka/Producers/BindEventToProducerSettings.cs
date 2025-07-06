namespace Bunker.MessageBus.Kafka.Producers;

public class BindEventToProducerSettings
{
    public string TargetTopic { get; set; } = "default";
    public string? EventName { get; set; }
    public Func<object, string?>? PartitionKeyFunction { get; set; }
}
