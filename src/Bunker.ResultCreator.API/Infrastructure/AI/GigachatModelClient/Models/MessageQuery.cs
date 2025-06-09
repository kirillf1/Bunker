using System.Text.Json.Serialization;

namespace Bunker.ResultCreator.API.Infrastructure.AI.GigachatModelClient.Models
{
    /// <summary>
    /// The MessageQuery model stores all request data. If you only need to pass
    /// messages, do not specify values for other arguments when initializing the class
    /// </summary>
    public class MessageQuery
    {
        /// <summary>
        /// Model name
        /// Default: GigaChat:latest
        /// </summary>
        [JsonPropertyName("model")]
        public string model { get; set; }

        /// <summary>
        /// Array of transmitted messages
        /// Default: -
        /// </summary>

        [JsonPropertyName("messages")]
        public List<MessageContent> messages { get; set; }

        /// <summary>
        /// Sampling temperature in the range from zero to two. The higher the value, the more random the model's response will be.
        /// Default: 0.87
        /// </summary>

        [JsonPropertyName("temperature")]
        public float temperature { get; set; }

        /// <summary>
        /// Parameter used as an alternative to temperature.
        /// Sets the probability mass of tokens that the model should consider.
        /// For example, if you pass a value of 0.1, the model will only consider tokens whose probability mass is in the top 10%.
        /// Default: 0.47
        /// </summary>
        [JsonPropertyName("top_p")]
        public float top_p { get; set; }

        /// <summary>
        /// Number of response variants to generate for each input message
        /// Default: 1
        /// </summary>
        [JsonPropertyName("n")]
        public long n { get; set; }

        /// <summary>
        /// Indicates that messages should be transmitted in parts in a stream.
        /// Default: false
        /// </summary>
        [JsonPropertyName("stream")]
        public bool stream { get; set; }

        /// <summary>
        /// Maximum number of tokens that will be used to generate responses
        /// Default: 512
        /// </summary>
        [JsonPropertyName("max_tokens")]
        public long max_tokens { get; set; }

        public MessageQuery(
            List<MessageContent>? messages = null,
            string model = "GigaChat:latest",
            float temperature = 0.87f,
            float top_p = 0.47f,
            long n = 1,
            bool stream = false,
            long max_tokens = 512
        )
        {
            var Contents = new List<MessageContent>();
            this.model = model;
            this.messages = messages ?? Contents;
            this.temperature = temperature;
            this.top_p = top_p;
            this.n = n;
            this.stream = stream;
            this.max_tokens = max_tokens;
        }
    }
}
