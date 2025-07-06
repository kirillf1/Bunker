using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;

namespace Bunker.MessageBus.Abstractions.Extensions;

public static class EventBusBuilderExtensions
{
    public static IMessageBusBuilder ConfigureJsonOptions(
        this IMessageBusBuilder eventBusBuilder,
        Action<JsonSerializerOptions> configure
    )
    {
        eventBusBuilder.Services.Configure<EventBusSubscriptionInfo>(o =>
        {
            configure(o.JsonSerializerOptions);
        });

        return eventBusBuilder;
    }

    public static IMessageBusBuilder AddSubscription<
        T,
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TH
    >(this IMessageBusBuilder eventBusBuilder, string? eventName = null)
        where T : IntegrationEvent
        where TH : class, IIntegrationEventHandler<T>
    {
        // Use keyed services to register multiple handlers for the same event type
        // the consumer can use IKeyedServiceProvider.GetKeyedService<IIntegrationEventHandler>(typeof(T)) to get all
        // handlers for the event type.
        eventBusBuilder.Services.AddKeyedTransient<IIntegrationEventHandler, TH>(typeof(T));

        eventBusBuilder.Services.Configure<EventBusSubscriptionInfo>(o =>
        {
            o.EventTypes[eventName ?? typeof(T).Name] = typeof(T);
        });

        return eventBusBuilder;
    }
}
