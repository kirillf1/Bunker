using System;
using System.Text.Json.Serialization;
using Bunker.GameComponents.API.Infrastructure.Database;
using Bunker.GameComponents.API.Infrastructure.OpenApi;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;

var builder = WebApplication.CreateBuilder(args);

builder
    .Services.AddControllers()
    .AddJsonOptions(opts =>
    {
        var enumConverter = new JsonStringEnumConverter();
        opts.JsonSerializerOptions.Converters.Add(enumConverter);
    });

builder.Services.AddSwaggerGen(c =>
{
    c.SchemaFilter<CardActionDtoSchemaFilter>();
});

builder.Services.AddDbContext<GameComponentsContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("PostgresConnection"));
    options.UseSnakeCaseNamingConvention();
    options.ReplaceService<IHistoryRepository, HistoryRepositoryWithChangedSchema>();
});

builder.Services.AddScoped<GameComponentsDatabaseInitializer>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

await using var scope = app.Services.CreateAsyncScope();
var dbInitializer = scope.ServiceProvider.GetRequiredService<GameComponentsDatabaseInitializer>();

await dbInitializer.InitializeAsync();

await app.RunAsync();
