using Bunker.Infrastructure.Shared.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Sinks.OpenTelemetry;

namespace Bunker.Infrastructure.Shared.Extensions
{
    public static class SerilogExtensions
    {
        public static IServiceCollection AddSerilogLogging(
            this IHostApplicationBuilder builder,
            string applicationName,
            string? instance = null
        )
        {
            var telemetryConfiguration = builder
                .Configuration.GetSection(OpenTelemetryOptions.Section)
                .Get<OpenTelemetryOptions>();

            var currentInstance = instance ?? $"{applicationName}-{OpenTelemetryExtensions.AppInstanceId}";

            builder.Services.AddSerilog(
                (services, lc) =>
                {
                    lc = lc
                        .ReadFrom.Configuration(builder.Configuration)
                        .ReadFrom.Services(services)
                        .Enrich.FromLogContext()
                        .WriteTo.Async(c => c.Console());

                    if (
                        telemetryConfiguration is not null
                        && !string.IsNullOrWhiteSpace(telemetryConfiguration.Logging.CollectorAddress)
                    )
                    {
                        lc.WriteTo.OpenTelemetry(opt =>
                        {
                            opt.Endpoint = telemetryConfiguration.Logging.CollectorAddress;
                            opt.Protocol = OtlpProtocol.Grpc;
                            opt.OnBeginSuppressInstrumentation = OpenTelemetry.SuppressInstrumentationScope.Begin;
                            opt.ResourceAttributes = new Dictionary<string, object>
                            {
                                ["service.name"] = applicationName,
                                ["service.instance.id"] = currentInstance,
                            };
                        });
                    }
                }
            );

            return builder.Services;
        }
    }
}
