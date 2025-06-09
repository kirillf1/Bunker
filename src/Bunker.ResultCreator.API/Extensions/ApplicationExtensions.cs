using System.Net;
using Bunker.ResultCreator.API.Domain.GameResultPrompts;
using Bunker.ResultCreator.API.Infrastructure.AI.GigachatModelClient;
using Bunker.ResultCreator.API.Infrastructure.AI.GigachatModelClient.Interfaces;
using Bunker.ResultCreator.API.Infrastructure.AI.GigachatModelClient.Options;
using Bunker.ResultCreator.API.Infrastructure.AI.Options;
using Bunker.ResultCreator.API.Infrastructure.PromptStorage;
using Bunker.ResultCreator.API.Services;
using Bunker.ResultCreator.API.SurvivalScenarioGenerators;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.Options;
using OllamaSharp;

namespace Bunker.ResultCreator.API.Extensions;

public static class ApplicationExtensions
{
    public static IServiceCollection AddApplicationServices(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        services.AddAIChatClient(configuration);
        services.AddPromptStorage(configuration);
        services.AddSurvivalScenarios();

        return services;
    }

    private static IServiceCollection AddAIChatClient(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<AIProviderOptions>(configuration.GetSection(AIProviderOptions.Section));

        var providerOptions =
            configuration.GetSection(AIProviderOptions.Section).Get<AIProviderOptions>() ?? new AIProviderOptions();

        if (providerOptions.Provider == AIProviderType.Ollama)
        {
            services.Configure<OllamaOptions>(configuration.GetSection(OllamaOptions.Section));

            services.AddSingleton<IChatClient>(serviceProvider =>
            {
                var options = serviceProvider.GetRequiredService<IOptions<OllamaOptions>>().Value;
                var uri = new Uri(options.BaseUrl);
                return new OllamaApiClient(uri, options.Model);
            });
        }
        else if (providerOptions.Provider == AIProviderType.GigaChat)
        {
            services.Configure<GigaChatOptions>(configuration.GetSection(GigaChatOptions.Section));

            services
                .AddHttpClient<GigaChatClient>(
                    (serviceProvider, client) =>
                    {
                        var options = serviceProvider.GetRequiredService<IOptions<GigaChatOptions>>().Value;
                        client.BaseAddress = new Uri(options.BaseUrl);
                    }
                )
                .ConfigurePrimaryHttpMessageHandler(serviceProvider =>
                {
                    var options = serviceProvider.GetRequiredService<IOptions<GigaChatOptions>>().Value;
                    return CreateHttpClientHandler(options.IgnoreTLS);
                });

            services
                .AddHttpClient<ITokenService, TokenService>()
                .ConfigurePrimaryHttpMessageHandler(serviceProvider =>
                {
                    var options = serviceProvider.GetRequiredService<IOptions<GigaChatOptions>>().Value;
                    return CreateHttpClientHandler(options.IgnoreTLS);
                });

            services.AddSingleton<IChatClient>(serviceProvider =>
            {
                var tokenService = serviceProvider.GetRequiredService<ITokenService>();
                var httpClientFactory = serviceProvider.GetRequiredService<IHttpClientFactory>();
                var gigaChatOptions = serviceProvider.GetRequiredService<IOptions<GigaChatOptions>>();

                var httpClient = httpClientFactory.CreateClient(nameof(GigaChatClient));
                return new GigaChatClient(tokenService, httpClient, gigaChatOptions);
            });
        }
        else
        {
            throw new InvalidOperationException($"Unsupported AI provider: {providerOptions.Provider}");
        }

        return services;
    }

    private static IServiceCollection AddPromptStorage(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<PromptStorageOptions>(configuration.GetSection(PromptStorageOptions.Section));
        services.AddSingleton<IPromptStorage, JsonPromptStorage>();
        return services;
    }

    private static IServiceCollection AddSurvivalScenarios(this IServiceCollection services)
    {
        services.AddScoped<ISurvivalScenarioGenerator, HybridWithAISurvivalScenarioGenerator>();
        services.AddScoped<IGameSessionResultService, GameSessionResultService>();

        return services;
    }

    private static HttpClientHandler CreateHttpClientHandler(bool ignoreTLS)
    {
        var handler = new HttpClientHandler();
        if (ignoreTLS)
        {
#pragma warning disable SYSLIB0014
#pragma warning disable S4830
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            ServicePointManager.ServerCertificateValidationCallback = (sender, cert, chain, sslPolicyErrors) => true;
            handler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true;
#pragma warning restore S4830
#pragma warning restore SYSLIB0014
        }
        return handler;
    }
}
