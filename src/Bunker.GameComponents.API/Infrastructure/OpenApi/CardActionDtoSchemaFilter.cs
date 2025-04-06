using System.Reflection;
using System.Text.Json.Serialization;
using Bunker.Domain.Shared.GameComponents;
using Bunker.GameComponents.API.Models.CharacterComponents.Cards.CardActions;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Bunker.GameComponents.API.Infrastructure.OpenApi;

#pragma warning disable CS8619
public class CardActionDtoSchemaFilter : ISchemaFilter
{
    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        if (context.Type != typeof(CardActionDto))
            return;

        schema.Properties.Clear();
        schema.Type = "object";

        var derivedTypes = GetDerivedTypesFromBaseClassAttributes(typeof(CardActionDto));
        var discriminatorValues = new Dictionary<string, string>();

        var oneOfSchemas = new List<OpenApiSchema>();
        foreach (var (type, discriminator) in derivedTypes)
        {
            string schemaRef = $"#/components/schemas/{type.Name}";
            discriminatorValues[discriminator] = schemaRef;

            // Генерируем схему для подтипа
            var derivedSchema = context.SchemaGenerator.GenerateSchema(type, context.SchemaRepository);

            // Добавляем общие поля (Id и $type)
            derivedSchema.Properties["Id"] = new OpenApiSchema { Type = "string", Format = "uuid" };
            derivedSchema.Properties["$type"] = new OpenApiSchema
            {
                Type = "string",
                Default = new OpenApiString(discriminator),
            };

            oneOfSchemas.Add(derivedSchema);
        }

        schema.Discriminator = new OpenApiDiscriminator { PropertyName = "$type", Mapping = discriminatorValues };

        schema.OneOf = oneOfSchemas;

        schema.Properties["$type"] = new OpenApiSchema
        {
            Type = "string",
            Enum = discriminatorValues.Keys.Select(k => new OpenApiString(k)).ToList<IOpenApiAny>(),
        };
        schema.Required = new HashSet<string> { "$type" };

        if (!schema.OneOf.Any())
        {
            schema.Description = "Warning: No derived types found for CardActionDto.";
        }
    }

    private static IEnumerable<(Type Type, string Discriminator)> GetDerivedTypesFromBaseClassAttributes(Type baseType)
    {
        var attributes = baseType.GetCustomAttributes<JsonDerivedTypeAttribute>(inherit: true);

        return attributes.Select(attr => (attr.DerivedType, attr.TypeDiscriminator?.ToString()));
    }
}
#pragma warning restore CS8619
