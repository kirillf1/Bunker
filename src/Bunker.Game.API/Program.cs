using System.Net;
using System.Text.Json.Serialization;
using Ardalis.Result;
using Ardalis.Result.AspNetCore;
using Bunker.Game.API.Extensions;
using Bunker.Game.Domain.AggregateModels.Characters.Cards;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

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
                        resultStatusOptions.For("POST", HttpStatusCode.Created).For("DELETE", HttpStatusCode.NoContent)
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

public partial class Program { }
