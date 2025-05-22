using System.Net;
using System.Text.Json.Serialization;
using Ardalis.Result;
using Ardalis.Result.AspNetCore;
using Bunker.Game.API.Extensions;
using Bunker.Game.Infrastructure.Application.Decorators;
using Bunker.Infrastructure.Shared.Extensions;
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

    // Add services to the container.
    var appName = "Bunker.Game.API";

    // Telemetry
    builder.AddBaseMetricsConfiguration(appName);
    builder.AddBaseTracingConfiguration(
        appName,
        null,
        ActivitySourceConstants.CommandsActivitySourceName,
        ActivitySourceConstants.QueriesActivitySourceName
    );
    builder.AddSerilogLogging(appName);

    builder.Services.AddControllers();

    builder.Services.AddOpenApi();

    builder.Services.AddApplicationServices(builder.Configuration);

    builder
        .Services.AddControllers(opt =>
        {
            opt.AddResultConvention(resultStatusMap =>
                resultStatusMap
                    .AddDefaultMap()
                    .For(
                        ResultStatus.Ok,
                        HttpStatusCode.OK,
                        resultStatusOptions =>
                            resultStatusOptions
                                .For("POST", HttpStatusCode.Created)
                                .For("DELETE", HttpStatusCode.NoContent)
                    )
            );
        })
        .AddJsonOptions(opts =>
        {
            opts.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        });

    builder.Services.ConfigureSwaggerGen(opt =>
    {
        opt.UseOneOfForPolymorphism();
    });
    builder.Services.AddSwaggerGen(c => { });

    var app = builder.Build();

    app.UseSerilogRequestLogging();

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
