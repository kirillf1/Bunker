﻿using System.Text.Json.Serialization;

namespace Bunker.ResultCreator.API.Infrastructure.AI.GigachatModelClient.Models
{
    public class EmbeddingResponse
    {
        [JsonPropertyName("object")]
        public string? Object { get; set; }

        [JsonPropertyName("data")]
        public List<EmbeddingData>? Data { get; set; }

        [JsonPropertyName("model")]
        public string? Model { get; set; }
    }
}
