using System.Text.Json.Serialization;

namespace Bunker.ResultCreator.API.Infrastructure.AI.GigachatModelClient.Models
{
    public class Model
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("object")]
        public string Object { get; set; }

        [JsonPropertyName("owned_by")]
        public string OwnedBy { get; set; }
    }
}
