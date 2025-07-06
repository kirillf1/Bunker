using System.Data;
using Bunker.Application.Shared.Contracts.IntegrationEvents.GameResults;
using Bunker.Application.Shared.CQRS;
using Bunker.Domain.Shared.Cards.CardActionCommands;
using Bunker.Domain.Shared.DomainEvents;
using Bunker.Game.Application.Commands.GameSessions.UpdateGameSessionState;
using Bunker.Game.Application.DomainEvents.GameSessions;
using Bunker.Game.Application.IntegrationEvents;
using Bunker.Game.Application.IntegrationEvents.GameSessionResultResponded;
using Bunker.Game.Domain.AggregateModels.Bunkers;
using Bunker.Game.Domain.AggregateModels.Catastrophes;
using Bunker.Game.Domain.AggregateModels.Characters;
using Bunker.Game.Domain.AggregateModels.Characters.Cards;
using Bunker.Game.Domain.AggregateModels.GameSessions;
using Bunker.Game.Infrastructure.Application;
using Bunker.Game.Infrastructure.Application.Decorators;
using Bunker.Game.Infrastructure.Application.IntegrationEvents;
using Bunker.Game.Infrastructure.Application.QueryHandlers;
using Bunker.Game.Infrastructure.Data;
using Bunker.Game.Infrastructure.Data.Repositories;
using Bunker.Game.Infrastructure.Generators.BunkerGenerators;
using Bunker.Game.Infrastructure.Generators.CatastropheGenerators;
using Bunker.Game.Infrastructure.Generators.CharacterFactories;
using Bunker.Game.Infrastructure.Generators.CharacteristicGenerators;
using Bunker.Game.Infrastructure.Http.GameComponents;
using Bunker.Game.Infrastructure.Http.GameComponents.Contracts;
using Bunker.Infrastructure.Shared.ApplicationDecorators;
using Bunker.MessageBus.Abstractions;
using Bunker.MessageBus.Abstractions.Extensions;
using Bunker.MessageBus.Abstractions.IntegrationEventLogs;
using Bunker.MessageBus.Kafka;
using Bunker.MessageBus.Kafka.Configuration;
using Bunker.MessageBus.Kafka.Consumers;
using Bunker.MessageBus.Kafka.Consumers.ConsumeStrategies;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;

namespace Bunker.Game.API.Extensions;

public static class ApplicationExtensions
{
    public static IServiceCollection AddApplicationServices(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        services.AddScoped<IDomainEventDispatcher, DomainEventDispatcher>();

        services.SubscribeToDomainEventsByAssembly(typeof(GameSessionCompletedDomainEventHandler).Assembly);

        services.AddDomainServices();

        services.AddCommandAndQueryHandlers();

        services.AddDatabase(configuration);

        services.AddGenerators(configuration);

        services.AddMessageBus(configuration);

        return services;
    }

    private static IMessageBusBuilder AddMessageBus(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<KafkaOptions>(configuration.GetSection(KafkaOptions.Section));

        var kafkaOptions = configuration.GetSection(KafkaOptions.Section).Get<KafkaOptions>();

        services
            .AddHealthChecks()
            .AddKafka(
                new Confluent.Kafka.ProducerConfig
                {
                    BootstrapServers = kafkaOptions!.Servers,
                    RequestTimeoutMs = 2000,
                    MessageSendMaxRetries = 10,
                    MessageTimeoutMs = 20000,
                },
                tags: ["ready", "startup"]
            );

        services.AddSingleton<IMessageBus>(c =>
        {
            var groupId = "bunker-game-api";
            var kafkaOptions = c.GetRequiredService<IOptions<KafkaOptions>>().Value;

            var builder = new KafkaBusBuilder(
                new KafkaConnectionSettings
                {
                    BootstrapServers = kafkaOptions.Servers,
                    SaslPassword = kafkaOptions.Password,
                    SaslUsername = kafkaOptions.Login,
                },
                c,
                c.GetRequiredService<IOptions<EventBusSubscriptionInfo>>()
            );

            builder = builder.AddEventConsumer(
                kafkaOptions.CreateGameResultResponsesTopicName,
                new ConsumerSettings(groupId, new MultiThreadForEventStrategy(5))
            );

            builder.BindEventToProducer<GameResultRequestedIntegrationEvent>(
                new MessageBus.Kafka.Producers.BindEventToProducerSettings
                {
                    TargetTopic = kafkaOptions.CreateGameResultRequestsTopicName,
                }
            );

            return builder.Build();
        });

        services.Configure<EventBusSubscriptionInfo>(c => c.JsonSerializerOptions.PropertyNameCaseInsensitive = true);

        services.AddSingleton<IHostedService>(sp => (KafkaMessageBus)sp.GetRequiredService<IMessageBus>());

        var messageBusBuilder = new MessageBusBuilder(services);

        messageBusBuilder.AddSubscription<
            GameResultRespondedIntegrationEvent,
            GameResultRespondedIntegrationEventHandler
        >();

        services.AddScoped<IBunkerGameIntegrationEventService, BunkerGameIntegrationEventService>();

        return messageBusBuilder;
    }

    private static IServiceCollection AddDomainServices(this IServiceCollection services)
    {
        services.Scan(scan =>
            scan.FromAssemblyOf<ICardCommandExecutor>()
                .AddClasses(classes => classes.AssignableTo(typeof(ICardActionCommandHandler<>)))
                .AsImplementedInterfaces()
                .WithScopedLifetime()
        );

        services.AddScoped<ICardCommandExecutor, CardCommandExecutor>();
        services.AddScoped<CharacterService>();

        return services;
    }

    private static IServiceCollection AddCommandAndQueryHandlers(this IServiceCollection services)
    {
        services.Scan(scan =>
            scan.FromAssemblyOf<EndGameSessionCommandHandler>()
                .AddClasses(classes => classes.AssignableTo(typeof(ICommandHandler<,>)))
                .AsImplementedInterfaces()
                .WithScopedLifetime()
        );

        services.Scan(scan =>
            scan.FromAssemblyOf<GetBunkerQueryHandler>()
                .AddClasses(classes => classes.AssignableTo(typeof(IQueryHandler<,>)))
                .AsImplementedInterfaces()
                .WithScopedLifetime()
        );

        services.Decorate(typeof(ICommandHandler<,>), typeof(ActivityCommandDecorator<,>));
        services.Decorate(typeof(ICommandHandler<,>), typeof(TransactionCommandDecorator<,>));

        services.Decorate(typeof(IQueryHandler<,>), typeof(ActivityQueryDecorator<,>));

        return services;
    }

    private static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<BunkerGameDbContext>(
            options =>
            {
                options.UseNpgsql(configuration.GetConnectionString("PostgresConnection"));
                options.UseSnakeCaseNamingConvention();
            },
            ServiceLifetime.Scoped
        );

        services
            .AddHealthChecks()
            .AddNpgSql(configuration.GetConnectionString("PostgresConnection")!, tags: ["ready", "startup"])
            .AddCheck("self", () => HealthCheckResult.Healthy(), tags: ["live"]);

        services.AddScoped<IDbConnection>(x =>
        {
            return x.GetRequiredService<BunkerGameDbContext>().Database.GetDbConnection();
        });
        services.AddScoped<BunkerGameDatabaseInitializer>();

        services.AddScoped<IGameSessionRepository, GameSessionRepository>();
        services.AddScoped<IBunkerRepository, BunkerRepository>();
        services.AddScoped<ICatastropheRepository, CatastropheRepository>();
        services.AddScoped<ICharacterRepository, CharacterRepository>();

        services.AddScoped<IIntegrationEventLogService, IntegrationEventLogService>();

        return services;
    }

    private static IServiceCollection AddGenerators(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<GameComponentsClientOptions>(configuration.GetSection(GameComponentsClientOptions.Section));

        var gameComponentsOptions = configuration
            .GetRequiredSection(GameComponentsClientOptions.Section)
            .Get<GameComponentsClientOptions>()!;

        services.AddHttpClient<ICharacterComponentsClient, CharacterComponentsClient>(client =>
        {
            client.BaseAddress = new Uri(gameComponentsOptions.Address);
        });

        services.AddHttpClient<IBunkerComponentsClient, BunkerComponentsClient>(client =>
        {
            client.BaseAddress = new Uri(gameComponentsOptions.Address);
        });

        services.AddHttpClient<ICatastropheComponentsClient, CatastropheComponentsClient>(client =>
        {
            client.BaseAddress = new Uri(gameComponentsOptions.Address);
        });

        services.AddScoped<ICharacteristicGenerator, CharacteristicGenerator>();
        services.AddScoped<IBunkerGenerator, BunkerGenerator>();
        services.AddScoped<ICatastropheGenerator, CatastropheGenerator>();
        services.AddScoped<ICharacterFactory, CharacterFactory>();

        return services;
    }
}
