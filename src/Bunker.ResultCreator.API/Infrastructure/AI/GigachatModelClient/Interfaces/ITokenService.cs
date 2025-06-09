using Bunker.ResultCreator.API.Infrastructure.AI.GigachatModelClient.Models;

namespace Bunker.ResultCreator.API.Infrastructure.AI.GigachatModelClient.Interfaces
{
    public interface ITokenService
    {
        Task<Token> CreateTokenAsync();
        long? ExpiresAt { get; }
        Token? Token { get; }
    }
}
