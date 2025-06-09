namespace Bunker.ResultCreator.API.Infrastructure.AI.GigachatModelClient.Options
{
    public class GigaChatOptions
    {
        public const string Section = "GigaChat";

        /// <summary>
        /// Secret key for authorization
        /// </summary>
        public string SecretKey { get; set; } = string.Empty;

        /// <summary>
        /// Commercial usage flag
        /// Default: false
        /// </summary>
        public bool IsCommercial { get; set; } = false;

        /// <summary>
        /// Flag to ignore TLS certificate validation
        /// Default: false
        /// </summary>
        public bool IgnoreTLS { get; set; } = false;

        /// <summary>
        /// Base URL for GigaChat API
        /// Default: https://gigachat.devices.sberbank.ru/api/v1/
        /// </summary>
        public string BaseUrl { get; set; } = "https://gigachat.devices.sberbank.ru/api/v1/";

        /// <summary>
        /// URL for token authorization
        /// Default: https://ngw.devices.sberbank.ru:9443/api/v2/oauth
        /// </summary>
        public string AuthUrl { get; set; } = "https://ngw.devices.sberbank.ru:9443/api/v2/oauth";

        /// <summary>
        /// Default model for requests
        /// Default: GigaChat-2
        /// </summary>
        public string DefaultModel { get; set; } = "GigaChat-2";

        /// <summary>
        /// Default request options
        /// </summary>
        public DefaultRequestOptions DefaultRequestOptions { get; set; } = new();
    }
}
