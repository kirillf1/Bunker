using System.Text.Json.Serialization;

namespace Bunker.ResultCreator.API.Infrastructure.AI.GigachatModelClient.Models
{
    public class Token
    {
        /// <summary>
        /// Access token
        /// </summary>
        [JsonPropertyName("access_token")]
        public string AccessToken { get; set; }

        /// <summary>
        /// Expiration time in Unix timestamp
        /// </summary>
        [JsonPropertyName("expires_at")]
        public long ExpiresAt { get; set; }

        [JsonConstructor]
        public Token() { }

        public Token(string access_token, long expires_at)
        {
            AccessToken = access_token;
            ExpiresAt = expires_at;
        }
    }
}
