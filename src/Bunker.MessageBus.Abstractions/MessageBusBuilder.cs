using Microsoft.Extensions.DependencyInjection;

namespace Bunker.MessageBus.Abstractions;

public class MessageBusBuilder(IServiceCollection services) : IMessageBusBuilder
{
    public IServiceCollection Services => services;
}
