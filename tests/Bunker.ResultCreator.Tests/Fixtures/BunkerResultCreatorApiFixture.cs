using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Bunker.ResultCreator.Tests.Fixtures;

public class BunkerResultCreatorApiFixture : WebApplicationFactory<Program>, IAsyncLifetime
{
    public BunkerResultCreatorApiFixture() { }

    protected override IHost CreateHost(IHostBuilder builder)
    {
        builder.ConfigureHostConfiguration(config =>
        {
            config.AddInMemoryCollection(new Dictionary<string, string?>());
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

        var logger = scope.ServiceProvider.GetRequiredService<ILogger<BunkerResultCreatorApiFixture>>();
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