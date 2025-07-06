using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using Bunker.ResultCreator.API.Infrastructure.AI.GigachatModelClient.Interfaces;
using Bunker.ResultCreator.API.Infrastructure.AI.GigachatModelClient.Models;
using Bunker.ResultCreator.API.Infrastructure.AI.GigachatModelClient.Options;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.Options;

namespace Bunker.ResultCreator.API.Infrastructure.AI.GigachatModelClient
{
    public class GigaChatClient : IChatClient, IDisposable
    {
        private bool _disposed = false;

        private ITokenService tokenService { get; }
        private HttpClient httpClient { get; }
        private GigaChatOptions options { get; }

        private string saveDirectory { get; set; } = Directory.GetCurrentDirectory();

        public GigaChatClient(ITokenService tokenService, HttpClient httpClient, IOptions<GigaChatOptions> options)
        {
            this.tokenService = tokenService ?? throw new ArgumentNullException(nameof(tokenService));
            this.httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            this.options = options?.Value ?? throw new ArgumentNullException(nameof(options));
        }

        public async Task<ChatResponse> GetResponseAsync(
            IEnumerable<ChatMessage> messages,
            ChatOptions? options = null,
            CancellationToken cancellationToken = default
        )
        {
            if (_disposed)
                throw new ObjectDisposedException(nameof(GigaChatClient));

            if (messages == null)
                throw new ArgumentNullException(nameof(messages));

            // Convert ChatMessage to MessageContent
            var messageContents = new List<MessageContent>();
            foreach (var message in messages)
            {
                var role = ConvertChatRoleToString(message.Role);
                var content = ExtractTextContent(message.Contents);
                messageContents.Add(new MessageContent(role, content));
            }

            // Create request with options
            var messageQuery = new MessageQuery(
                messages: messageContents,
                model: options?.ModelId ?? this.options.DefaultModel,
                temperature: options?.Temperature ?? this.options.DefaultRequestOptions.Temperature,
                max_tokens: options?.MaxOutputTokens ?? this.options.DefaultRequestOptions.MaxTokens
            );

            try
            {
                var response = await CompletionsAsync(messageQuery);

                if (response?.Choices == null || response.Choices.Count == 0)
                    throw new InvalidOperationException("Received empty response from GigaChat API");

                var choice = response.Choices[0];
                var responseMessage = choice.Message?.Content ?? string.Empty;

                // Create ChatResponse
                var chatMessage = new ChatMessage(ChatRole.Assistant, responseMessage);
                var chatResponse = new ChatResponse(chatMessage)
                {
                    ModelId = response.Model,
                    FinishReason = ConvertFinishReason(choice.FinishReason),
                    Usage = new UsageDetails
                    {
                        InputTokenCount = response.Usage?.PromptTokens,
                        OutputTokenCount = response.Usage?.CompletionTokens,
                        TotalTokenCount = response.Usage?.TotalTokens,
                    },
                };

                return chatResponse;
            }
            catch (Exception ex)
            {
                throw new ApplicationException(
                    $"Error occurred while getting response from GigaChat: {ex.Message}",
                    ex
                );
            }
        }

        public async IAsyncEnumerable<ChatResponseUpdate> GetStreamingResponseAsync(
            IEnumerable<ChatMessage> messages,
            ChatOptions? chatOptions = null,
            [EnumeratorCancellation] CancellationToken cancellationToken = default
        )
        {
            if (_disposed)
                throw new ObjectDisposedException(nameof(GigaChatClient));

            var messageContents = new List<MessageContent>();
            foreach (var message in messages)
            {
                var role = ConvertChatRoleToString(message.Role);
                var content = ExtractTextContent(message.Contents);
                messageContents.Add(new MessageContent(role, content));
            }

            var messageQuery = new MessageQuery(
                messages: messageContents,
                model: chatOptions?.ModelId ?? options.DefaultModel,
                temperature: chatOptions?.Temperature ?? options.DefaultRequestOptions.Temperature,
                max_tokens: chatOptions?.MaxOutputTokens ?? options.DefaultRequestOptions.MaxTokens
            );

            var response = await CompletionsAsync(messageQuery);

            if (response?.Choices != null && response.Choices.Count > 0)
            {
                var choice = response.Choices[0];
                var responseMessage = choice.Message?.Content ?? string.Empty;

                yield return new ChatResponseUpdate
                {
                    ModelId = response.Model,
                    FinishReason = ConvertFinishReason(choice.FinishReason),
                    Contents = [new TextContent(responseMessage)],
                };
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private async Task<Token> CreateTokenAsync()
        {
            return await tokenService.CreateTokenAsync();
        }

        private async Task<Response?> CompletionsAsync(MessageQuery query)
        {
            await ValidateToken();

            var request = new HttpRequestMessage(HttpMethod.Post, "chat/completions");
            request.Headers.Add("Authorization", $"Bearer {tokenService.Token.AccessToken}");
            request.Content = new StringContent(JsonSerializer.Serialize(query), Encoding.UTF8, "application/json");

            var response = await httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
            var responseBody = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<Response>(responseBody);
        }

        private async Task<Response?> CompletionsAsync(string message)
        {
            if (string.IsNullOrWhiteSpace(message))
                throw new ArgumentException("Message cannot be empty.", nameof(message));

            var query = new MessageQuery();
            query.messages.Add(new MessageContent("user", message));
            return await CompletionsAsync(query);
        }

        private async Task<EmbeddingResponse?> EmbeddingAsync(EmbeddingRequest request)
        {
            try
            {
                await ValidateToken();

                var httpRequest = new HttpRequestMessage(HttpMethod.Post, "embeddings");
                httpRequest.Headers.Add("Authorization", $"Bearer {tokenService.Token.AccessToken}");
                httpRequest.Content = new StringContent(
                    JsonSerializer.Serialize(request),
                    Encoding.UTF8,
                    "application/json"
                );

                var response = await httpClient.SendAsync(httpRequest);
                response.EnsureSuccessStatusCode();
                var responseBody = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<EmbeddingResponse>(responseBody);
            }
            catch (JsonException ex)
            {
                throw new ApplicationException("Failed to deserialize response.", ex);
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Unknown error occurred while getting embedding: {ex.Message}", ex);
            }
        }

        private async Task ValidateToken()
        {
            if (
                !tokenService.ExpiresAt.HasValue
                || tokenService.ExpiresAt.Value < ((DateTimeOffset)DateTime.UtcNow).ToUnixTimeSeconds()
            )
                await CreateTokenAsync();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed && disposing)
                _disposed = true;
        }

        private string ConvertChatRoleToString(ChatRole role)
        {
            return role.Value switch
            {
                "user" => "user",
                "assistant" => "assistant",
                "system" => "system",
                "tool" => "tool",
                _ => "user",
            };
        }

        private string ExtractTextContent(IList<AIContent> contents)
        {
            var textBuilder = new StringBuilder();
            foreach (var content in contents)
            {
                if (content is TextContent textContent)
                    textBuilder.AppendLine(textContent.Text);
            }
            return textBuilder.ToString().Trim();
        }

        private ChatFinishReason? ConvertFinishReason(string? finishReason)
        {
            return finishReason switch
            {
                "stop" => ChatFinishReason.Stop,
                "length" => ChatFinishReason.Length,
                "tool_calls" => ChatFinishReason.ToolCalls,
                "content_filter" => ChatFinishReason.ContentFilter,
                _ => null,
            };
        }

        public object? GetService(Type serviceType, object? serviceKey = null)
        {
            throw new NotImplementedException();
        }
    }
}
