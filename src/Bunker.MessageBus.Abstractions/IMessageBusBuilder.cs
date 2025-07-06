using Microsoft.Extensions.DependencyInjection;

namespace Bunker.MessageBus.Abstractions;

public interface IMessageBusBuilder
{
    public IServiceCollection Services { get; }
}
