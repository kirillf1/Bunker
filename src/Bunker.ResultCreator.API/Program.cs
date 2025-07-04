using Bunker.Infrastructure.Shared.Extensions;
using Bunker.ResultCreator.API.Extensions;
using Serilog;
using Serilog.Events;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .CreateBootstrapLogger();

try
{
    var builder = WebApplication.CreateBuilder(args);

    var appName = "Bunker.ResultCreator.API";

    // Telemetry
    builder.AddBaseMetricsConfiguration(appName);
    builder.AddBaseTracingConfiguration(appName);
    builder.AddSerilogLogging(appName);

    builder.Services.AddControllers();

    builder.Services.AddSwaggerGen(c => { });

    builder.Services.AddOpenApi();

    builder.Services.AddApplicationServices(builder.Configuration);

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();

    app.UseAuthorization();

    app.MapControllers();

    await app.RunAsync();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Server terminated unexpectedly");
}
finally
{
    await Log.CloseAndFlushAsync();
}

public partial class Program { }
