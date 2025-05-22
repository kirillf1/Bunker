using System.Reflection;
using System.Text.Json.Serialization;
using Bunker.GameComponents.API.Infrastructure.Database;
using Bunker.Infrastructure.Shared.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
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

    builder
        .Services.AddControllers()
        .AddJsonOptions(opts =>
        {
            opts.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        });

    // Add services to the container.
    var appName = "Bunker.GameComponents.API";

    // Telemetry
    builder.AddBaseMetricsConfiguration(appName);
    builder.AddBaseTracingConfiguration(appName);
    builder.AddSerilogLogging(appName);

    builder.Services.ConfigureSwaggerGen(opt =>
    {
        opt.UseOneOfForPolymorphism();

        opt.SelectDiscriminatorNameUsing(_ => "$type");
        opt.SelectDiscriminatorValueUsing(subType =>
            subType
                .BaseType!.GetCustomAttributes<JsonDerivedTypeAttribute>()
                .FirstOrDefault(x => x.DerivedType == subType)
                ?.TypeDiscriminator!.ToString()
        );
    });
    builder.Services.AddSwaggerGen(c => { });

    builder.Services.AddDbContext<GameComponentsContext>(options =>
    {
        options.UseNpgsql(builder.Configuration.GetConnectionString("PostgresConnection"));
        options.UseSnakeCaseNamingConvention();
        options.ReplaceService<IHistoryRepository, HistoryRepositoryWithChangedSchema>();
    });

    builder.Services.AddScoped<GameComponentsDatabaseInitializer>();

    var app = builder.Build();

    app.UseSerilogRequestLogging();

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseAuthorization();

    app.MapControllers();

    await using var scope = app.Services.CreateAsyncScope();
    var dbInitializer = scope.ServiceProvider.GetRequiredService<GameComponentsDatabaseInitializer>();

    await dbInitializer.InitializeAsync();

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
