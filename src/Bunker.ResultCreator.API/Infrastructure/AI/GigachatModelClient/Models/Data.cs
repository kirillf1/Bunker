using System.Text.Json.Serialization;

namespace Bunker.ResultCreator.API.Infrastructure.AI.GigachatModelClient.Models
{
    public class Data
    {
        [JsonPropertyName("data")]
        public MessageQuery data { get; set; }

        public Data()
        {
            data = new MessageQuery();
        }
    }
}
