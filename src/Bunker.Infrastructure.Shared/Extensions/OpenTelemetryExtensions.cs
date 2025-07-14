using Bunker.Infrastructure.Shared.ApplicationDecorators;
using Bunker.Infrastructure.Shared.Options;
using HealthChecks.OpenTelemetry.Instrumentation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Npgsql;
using OpenTelemetry;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace Bunker.Infrastructure.Shared.Extensions
{
    public static class OpenTelemetryExtensions
    {
        internal static Guid AppInstanceId = Guid.NewGuid();

        public static OpenTelemetryBuilder AddBaseTracingConfiguration(
            this IHostApplicationBuilder builder,
            string applicationName,
            string? instance = null,
            params string[] sources
        )
        {
            var otel = builder.Services.AddOpenTelemetry();

            var currentInstance = instance ?? $"{applicationName}-{AppInstanceId}";

            var telemetryConfiguration = builder
                .Configuration.GetSection(OpenTelemetryOptions.Section)
                .Get<OpenTelemetryOptions>();

            if (telemetryConfiguration is null)
                return otel;

            otel.WithTracing(builder =>
            {
                builder
                    .AddAspNetCoreInstrumentation()
                    .AddHttpClientInstrumentation()
                    .AddRedisInstrumentation()
                    .SetResourceBuilder(
                        ResourceBuilder
                            .CreateDefault()
                            .AddService(
                                serviceName: applicationName,
                                autoGenerateServiceInstanceId: false,
                                serviceInstanceId: currentInstance
                            )
                    )
                    .AddSource(sources)
                    .AddNpgsql()
                    .AddAspNetCoreInstrumentation()
                    .AddSource("Confluent.Kafka.Extensions.Diagnostics")
                    .AddSource(
                        ActivitySourceConstants.CommandsActivitySourceName,
                        ActivitySourceConstants.QueriesActivitySourceName
                    );

                if (!string.IsNullOrEmpty(telemetryConfiguration.Tracing.CollectorAddress))
                {
                    builder.AddOtlpExporter(opts =>
                    {
                        opts.Endpoint = new Uri(telemetryConfiguration.Tracing.CollectorAddress);
                    });
                }
            });

            return otel;
        }

        public static OpenTelemetryBuilder AddBaseMetricsConfiguration(
            this IHostApplicationBuilder builder,
            string applicationName,
            string? instance = null,
            params string[] extraMeters
        )
        {
            var otel = builder.Services.AddOpenTelemetry();

            builder.Services.AddHealthChecks();

            var currentInstance = instance ?? $"{applicationName}-{AppInstanceId}";

            var telemetryConfiguration = builder
                .Configuration.GetSection(OpenTelemetryOptions.Section)
                .Get<OpenTelemetryOptions>();

            if (telemetryConfiguration is null)
                return otel;

            if (!string.IsNullOrEmpty(telemetryConfiguration.Metrics.CollectorAddress))
            {
                _ = otel.WithMetrics(opts =>
                    opts.SetResourceBuilder(
                            ResourceBuilder
                                .CreateDefault()
                                .AddService(
                                    serviceName: applicationName,
                                    autoGenerateServiceInstanceId: false,
                                    serviceInstanceId: instance
                                )
                        )
                        .AddProcessInstrumentation()
                        .AddAspNetCoreInstrumentation()
                        .AddRuntimeInstrumentation()
                        .AddHealthChecksInstrumentation()
                        .AddHttpClientInstrumentation()
                        .AddMeter(extraMeters)
                        .AddOtlpExporter(opts =>
                        {
                            opts.Endpoint = new Uri(telemetryConfiguration.Metrics.CollectorAddress);
                        })
                );
            }
            return otel;
        }
    }
}
