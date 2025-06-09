using System.Text.Json.Serialization;

namespace Bunker.ResultCreator.API.Infrastructure.AI.GigachatModelClient.Models
{
    /// <summary>
    /// The MessageContent class stores all data for a specific message
    /// </summary>
    public class MessageContent
    {
        /// <summary>
        /// Sender role
        /// </summary>

        [JsonPropertyName("role")]
        public string Role { get; set; }

        /// <summary>
        /// Message text
        /// </summary>

        [JsonPropertyName("content")]
        public string Content { get; set; }

        public MessageContent(string role, string content)
        {
            Role = role;
            Content = content;
        }
    }
}
