namespace Bunker.MessageBus.Kafka;

public class KafkaConnectionSettings
{
    public string BootstrapServers { get; set; } = string.Empty;
    public string SaslPassword { get; set; } = string.Empty;
    public string SaslUsername { get; set; } = string.Empty;
}
