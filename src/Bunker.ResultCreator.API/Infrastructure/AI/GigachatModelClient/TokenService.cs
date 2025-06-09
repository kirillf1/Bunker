using System.Net.Http.Headers;
using System.Text.Json;
using Bunker.ResultCreator.API.Infrastructure.AI.GigachatModelClient.Interfaces;
using Bunker.ResultCreator.API.Infrastructure.AI.GigachatModelClient.Models;
using Bunker.ResultCreator.API.Infrastructure.AI.GigachatModelClient.Options;
using Microsoft.Extensions.Options;

namespace Bunker.ResultCreator.API.Infrastructure.AI.GigachatModelClient
{
    public class TokenService : ITokenService
    {
        private readonly HttpClient httpClient;
        private readonly GigaChatOptions options;
        public long? ExpiresAt { get; private set; }
        public Token? Token { get; private set; }

        public TokenService(HttpClient httpClient, IOptions<GigaChatOptions> options)
        {
            this.httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            this.options = options?.Value ?? throw new ArgumentNullException(nameof(options));
        }

        public async Task<Token> CreateTokenAsync()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(options.SecretKey))
                    throw new InvalidOperationException("Secret key is missing.");

                var request = new HttpRequestMessage(HttpMethod.Post, options.AuthUrl);
                request.Headers.Add("Authorization", "Bearer " + options.SecretKey);
                request.Headers.Add("RqUID", Guid.NewGuid().ToString());

                if (options.IsCommercial == true)
                    request.Content = new StringContent("scope=GIGACHAT_API_CORP");
                else
                {
                    request.Content = new StringContent("scope=GIGACHAT_API_PERS");
                }
                request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");

                var response = await httpClient.SendAsync(request);
                response.EnsureSuccessStatusCode();
                var responseBody = await response.Content.ReadAsStringAsync();

                Token =
                    JsonSerializer.Deserialize<Token>(responseBody)
                    ?? throw new JsonException("Failed to deserialize token response.");

                ExpiresAt = Token.ExpiresAt;
                return Token;
            }
            catch (JsonException ex)
            {
                throw new ApplicationException("Failed to deserialize token response.", ex);
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Unknown error occurred while creating token: {ex.Message}", ex);
            }
        }
    }
}
