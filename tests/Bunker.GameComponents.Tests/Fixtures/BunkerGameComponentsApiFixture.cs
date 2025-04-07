using Bunker.GameComponents.API.Infrastructure.Database;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Bunker.GameComponents.Tests.Fixtures;

public class BunkerGameComponentsApiFixture : WebApplicationFactory<Program>, IAsyncLifetime
{
    protected override IHost CreateHost(IHostBuilder builder)
    {
        builder.ConfigureHostConfiguration(config =>
        {
            config.AddInMemoryCollection(new Dictionary<string, string?> { });
        });
        builder.ConfigureServices(services => { });

        return base.CreateHost(builder);
    }

    public Task InitializeAsync()
    {
        return Task.CompletedTask;
    }

    async Task IAsyncLifetime.DisposeAsync()
    {
        using var scope = Services.CreateScope();

        var logger = scope.ServiceProvider.GetRequiredService<ILogger<BunkerGameComponentsApiFixture>>();
        try
        {
            var context = scope.ServiceProvider.GetRequiredService<GameComponentsContext>();

            //await context.Database.EnsureDeletedAsync();
            await base.DisposeAsync();

            logger.LogInformation("Test app disposed");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Fail dispose test app");
        }
    }
}
