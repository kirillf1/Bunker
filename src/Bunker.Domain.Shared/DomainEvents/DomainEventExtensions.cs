using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace Bunker.Domain.Shared.DomainEvents;

public static class DomainEventExtensions
{
    public static void SubscribeToDomainEventsByAssembly(this IServiceCollection services, Assembly assembly)
    {
        var handlerInterfaceType = typeof(IDomainEventHandler<>);

        var handlers = assembly
            .GetTypes()
            .Where(x => !x.IsAbstract && !x.IsInterface)
            .SelectMany(x => x.GetInterfaces(), (type, iface) => new { type, iface })
            .Where(x => x.iface.IsGenericType && x.iface.GetGenericTypeDefinition() == handlerInterfaceType)
            .ToList();

        foreach (var handler in handlers)
        {
            var eventType = handler.iface.GetGenericArguments()[0];
            var handlerType = handler.type;

            services.AddKeyedTransient(typeof(IDomainEventHandler), eventType, handlerType);
        }
    }

    public static void SubscribeToDomainEvent<T, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TH>(
        this IServiceCollection services
    )
        where T : IDomainEvent
        where TH : class, IDomainEventHandler<T>
    {
        services.AddKeyedTransient<IDomainEventHandler, TH>(typeof(T));
    }
}
