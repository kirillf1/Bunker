using System.Data;
using Bunker.Application.Shared.CQRS;
using Bunker.Game.Application.Queries.Bunkers;

namespace Bunker.Game.Infrastructure.Application.QueryHandlers
{
    public class GetBunkerQueryHandler : IQueryHandler<GetBunkerQuery, BunkerDto?>
    {
        private readonly IDbConnection _dbConnection;

        public GetBunkerQueryHandler(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public Task<BunkerDto?> Handle(GetBunkerQuery query, CancellationToken cancellation)
        {
            return null;
        }
    }
}
