using System.Text.Json;
using System.Text.Json.Serialization;
using Bunker.Game.Domain.AggregateModels.Characters.Cards.CardActions;

namespace Bunker.Game.Infrastructure.Data.Converters;

public class CardActionJsonConverter : JsonConverter<CardAction>
{
    private const string TypeDiscriminator = "$type";

    private static readonly Dictionary<string, Type> _typeMappings =
        new()
        {
            { "AddCharacteristicCardAction", typeof(AddCharacteristicCardAction) },
            { "SpyCharacteristicCardAction", typeof(SpyCharacteristicCardAction) },
            { "RemoveCharacteristicCardAction", typeof(RemoveCharacteristicCardAction) },
            { "RerollCharacteristicCardAction", typeof(RerollCharacteristicCardAction) },
            { "RevealBunkerGameComponentCardAction", typeof(RevealBunkerGameComponentCardAction) },
            { "EmptyAction", typeof(EmptyAction) },
            { "ExchangeCharacteristicAction", typeof(ExchangeCharacteristicAction) },
            { "RecreateBunkerAction", typeof(RecreateBunkerAction) },
            { "RecreateCharacterAction", typeof(RecreateCharacterAction) },
            { "RecreateCatastropheAction", typeof(RecreateCatastropheAction) },
        };

    public override CardAction? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartObject)
        {
            throw new JsonException("Expected start of object");
        }

        using var jsonDoc = JsonDocument.ParseValue(ref reader);
        var rootElement = jsonDoc.RootElement;

        if (!rootElement.TryGetProperty(TypeDiscriminator, out var typeProperty))
        {
            throw new JsonException($"Missing type discriminator property '{TypeDiscriminator}'");
        }

        var typeName = typeProperty.GetString();
        if (string.IsNullOrEmpty(typeName) || !_typeMappings.TryGetValue(typeName, out var type))
        {
            throw new JsonException($"Unknown or unsupported card action type: {typeName}");
        }

        var newOptions = new JsonSerializerOptions(options);
        newOptions.Converters.Remove(this);

        var enumConverter = options.Converters.FirstOrDefault(c => c is JsonStringEnumConverter);
        if (enumConverter != null)
        {
            newOptions.Converters.Add(enumConverter);
        }

        var jsonString = JsonSerializer.Serialize(rootElement, new JsonSerializerOptions { WriteIndented = false });

        return (CardAction)JsonSerializer.Deserialize(jsonString, type, newOptions)!;
    }

    public override void Write(Utf8JsonWriter writer, CardAction value, JsonSerializerOptions options)
    {
        if (value is null)
        {
            writer.WriteNullValue();
            return;
        }

        var type = value.GetType();
        var typeName = type.Name;

        var newOptions = new JsonSerializerOptions(options);
        newOptions.Converters.Remove(this);

        var enumConverter = options.Converters.FirstOrDefault(c => c is JsonStringEnumConverter);
        if (enumConverter != null)
        {
            newOptions.Converters.Add(enumConverter);
        }

        var jsonString = JsonSerializer.Serialize(value, type, newOptions);
        using var jsonDocument = JsonDocument.Parse(jsonString);

        writer.WriteStartObject();

        writer.WriteString(TypeDiscriminator, typeName);

        foreach (var property in jsonDocument.RootElement.EnumerateObject())
        {
            property.WriteTo(writer);
        }

        writer.WriteEndObject();
    }
}
