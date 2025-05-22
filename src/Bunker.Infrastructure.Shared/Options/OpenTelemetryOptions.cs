namespace Bunker.Infrastructure.Shared.Options
{
    public class OpenTelemetryOptions
    {
        public const string Section = "OpenTelemetry";

        public LoggingOptions Logging { get; set; } = new LoggingOptions();
        public MetricsOptions Metrics { get; set; } = new MetricsOptions();
        public TracingOptions Tracing { get; set; } = new TracingOptions();

        public class LoggingOptions
        {
            public string CollectorAddress { get; set; } = string.Empty;
        }

        public class MetricsOptions
        {
            public string CollectorAddress { get; set; } = string.Empty;
        }

        public class TracingOptions
        {
            public string CollectorAddress { get; set; } = string.Empty;
        }
    }
}
