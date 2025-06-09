using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using Bunker.ResultCreator.API.Domain.SurvivalPredictor;

namespace Bunker.ResultCreator.API.Infrastructure.Json;

public partial class AISurvivalCapabilityResponseJsonConverter : JsonConverter<BunkerSurvivalCapabilityResult>
{
    public override BunkerSurvivalCapabilityResult? Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options
    )
    {
        using var doc = JsonDocument.ParseValue(ref reader);
        var rawJson = doc.RootElement.GetRawText();

        var jsonMatch = JsonRegex().Match(rawJson);

        if (!jsonMatch.Success)
            throw new JsonException("Json not found.");

        var cleanJson = jsonMatch.Value;

        var optionsWithoutConverter = new JsonSerializerOptions(options);
        var thisConverter = optionsWithoutConverter.Converters.FirstOrDefault(x =>
            x.GetType() == typeof(AISurvivalCapabilityResponseJsonConverter)
        );
        if (thisConverter is not null)
            optionsWithoutConverter.Converters.Remove(thisConverter);

        return JsonSerializer.Deserialize<BunkerSurvivalCapabilityResult>(cleanJson, optionsWithoutConverter);
    }

    public override void Write(
        Utf8JsonWriter writer,
        BunkerSurvivalCapabilityResult value,
        JsonSerializerOptions options
    )
    {
        JsonSerializer.Serialize(writer, value, options);
    }

    [GeneratedRegex(@"\{[\s\S]*?\}")]
    private static partial Regex JsonRegex();
}
