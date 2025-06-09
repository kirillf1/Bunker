using System.Text.Json.Serialization;

namespace Bunker.ResultCreator.API.Infrastructure.AI.GigachatModelClient.Models
{
    public class Choice
    {
        [JsonPropertyName("message")]
        public MessageContent? Message { get; set; }

        [JsonPropertyName("index")]
        public int Index { get; set; }

        [JsonPropertyName("finish_reason")]
        public string? FinishReason { get; set; }
    }
}
