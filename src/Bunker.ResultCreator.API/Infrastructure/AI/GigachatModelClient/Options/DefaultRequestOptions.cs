namespace Bunker.ResultCreator.API.Infrastructure.AI.GigachatModelClient.Options
{
    public class DefaultRequestOptions
    {
        /// <summary>
        /// Sampling temperature in the range from zero to two
        /// Default: 0.87
        /// </summary>
        public float Temperature { get; set; } = 0.87f;

        /// <summary>
        /// Parameter used as an alternative to temperature
        /// Default: 0.47
        /// </summary>
        public float TopP { get; set; } = 0.47f;

        /// <summary>
        /// Maximum number of tokens for response
        /// Default: 512
        /// </summary>
        public long MaxTokens { get; set; } = 512;

        /// <summary>
        /// Number of response variations
        /// Default: 1
        /// </summary>
        public long N { get; set; } = 1;

        /// <summary>
        /// Indicates that messages should be transmitted in parts in a stream
        /// Default: false
        /// </summary>
        public bool Stream { get; set; } = false;
    }
} 
