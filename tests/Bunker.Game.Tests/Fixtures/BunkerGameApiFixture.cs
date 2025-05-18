using Bunker.Game.Domain.AggregateModels.Bunkers;
using Bunker.Game.Domain.AggregateModels.Catastrophes;
using Bunker.Game.Domain.AggregateModels.Characters;
using Bunker.Game.Tests.Fakes;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Bunker.Game.Tests.Fixtures;

public class BunkerGameApiFixture : WebApplicationFactory<Program>, IAsyncLifetime
{
    public BunkerGameApiFixture() { }

    protected override IHost CreateHost(IHostBuilder builder)
    {
        builder.ConfigureHostConfiguration(config =>
        {
            config.AddInMemoryCollection(new Dictionary<string, string?>());
        });

        builder.ConfigureServices(services =>
        {
            var bunkerDescriptor = services.SingleOrDefault(d => d.ServiceType == typeof(IBunkerGenerator));
            if (bunkerDescriptor != null)
                services.Remove(bunkerDescriptor);

            var catastropheDescriptor = services.SingleOrDefault(d => d.ServiceType == typeof(ICatastropheGenerator));
            if (catastropheDescriptor != null)
                services.Remove(catastropheDescriptor);

            var characteristicDescriptor = services.SingleOrDefault(d =>
                d.ServiceType == typeof(ICharacteristicGenerator)
            );

            if (characteristicDescriptor != null)
                services.Remove(characteristicDescriptor);

            services.AddSingleton<IBunkerGenerator, MockBunkerGenerator>();
            services.AddSingleton<ICatastropheGenerator, MockCatastropheGenerator>();
            services.AddSingleton<ICharacteristicGenerator, MockCharacteristicGenerator>();
        });

        return base.CreateHost(builder);
    }

    public Task InitializeAsync()
    {
        return Task.CompletedTask;
    }

    async Task IAsyncLifetime.DisposeAsync()
    {
        using var scope = Services.CreateScope();

        var logger = scope.ServiceProvider.GetRequiredService<ILogger<BunkerGameApiFixture>>();
        try
        {
            await base.DisposeAsync();

            logger.LogInformation("Test app disposed");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to dispose test app");
        }
    }
}
