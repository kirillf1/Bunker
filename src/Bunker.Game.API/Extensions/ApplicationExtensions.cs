using System.Data;
using Bunker.Application.Shared.CQRS;
using Bunker.Domain.Shared.Cards.CardActionCommands;
using Bunker.Domain.Shared.DomainEvents;
using Bunker.Game.Application.Commands.GameSessions.UpdateGameSessionState;
using Bunker.Game.Application.DomainEvents.GameSessions;
using Bunker.Game.Domain.AggregateModels.Bunkers;
using Bunker.Game.Domain.AggregateModels.Catastrophes;
using Bunker.Game.Domain.AggregateModels.Characters;
using Bunker.Game.Domain.AggregateModels.Characters.Cards;
using Bunker.Game.Domain.AggregateModels.GameSessions;
using Bunker.Game.Infrastructure.Application;
using Bunker.Game.Infrastructure.Application.Decorators;
using Bunker.Game.Infrastructure.Application.QueryHandlers;
using Bunker.Game.Infrastructure.Data;
using Bunker.Game.Infrastructure.Data.Repositories;
using Bunker.Game.Infrastructure.Generators.BunkerGenerators;
using Bunker.Game.Infrastructure.Generators.CatastropheGenerators;
using Bunker.Game.Infrastructure.Generators.CharacterFactories;
using Bunker.Game.Infrastructure.Generators.CharacteristicGenerators;
using Bunker.Game.Infrastructure.Http.GameComponents;
using Bunker.Game.Infrastructure.Http.GameComponents.Contracts;
using Microsoft.EntityFrameworkCore;

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

        return services;
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
        services.AddDbContext<BunkerGameDbContext>(options =>
        {
            options.UseNpgsql(configuration.GetConnectionString("PostgresConnection"));
            options.UseSnakeCaseNamingConvention();
        });

        services.AddScoped<IDbConnection>(x =>
        {
            return x.GetRequiredService<BunkerGameDbContext>().Database.GetDbConnection();
        });

        services.AddScoped<IGameSessionRepository, GameSessionRepository>();
        services.AddScoped<IBunkerRepository, BunkerRepository>();
        services.AddScoped<ICatastropheRepository, CatastropheRepository>();
        services.AddScoped<ICharacterRepository, CharacterRepository>();

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
