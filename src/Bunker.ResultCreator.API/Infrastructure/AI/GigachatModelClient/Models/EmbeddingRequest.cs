using System.Text.Json.Serialization;

namespace Bunker.ResultCreator.API.Infrastructure.AI.GigachatModelClient.Models
{
    public class EmbeddingRequest
    {
        [JsonPropertyName("models")]
        public string Models { get; set; }

        [JsonPropertyName("input")]
        public List<string> Input { get; set; }

        public EmbeddingRequest(string models = "Embeddings", List<string> input = null)
        {
            var inputs = new List<string>();
            Models = models;
            Input = input ?? inputs;
        }
    }
}
