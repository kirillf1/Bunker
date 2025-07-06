namespace Bunker.MessageBus.Abstractions;

public interface IMessageBus
{
    Task PublishAsync(IntegrationEvent @event);
    Task PublishAsync(IEnumerable<IntegrationEvent> events);
}
