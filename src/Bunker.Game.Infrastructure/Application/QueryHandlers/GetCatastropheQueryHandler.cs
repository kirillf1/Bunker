using System.Data;
using Ardalis.Result;
using Bunker.Game.Application.Queries.Catastrophes;
using Dapper;

namespace Bunker.Game.Infrastructure.Application.QueryHandlers;

public class GetCatastropheQueryHandler : IQueryHandler<GetCatastropheQuery, Result<CatastropheDto>>
{
    private readonly IDbConnection _dbConnection;

    public GetCatastropheQueryHandler(IDbConnection dbConnection)
    {
        _dbConnection = dbConnection;
    }

    public async Task<Result<CatastropheDto>> Handle(GetCatastropheQuery query, CancellationToken cancellation)
    {
        var sql =
            $@"
        SELECT
            id AS {nameof(CatastropheDto.Id)},
            description AS {nameof(CatastropheDto.Description)},
            is_read_only AS {nameof(CatastropheDto.IsReadonly)}
        FROM catastrophes
        WHERE game_session_id = @gameSessionId";

        var catastropheDTO = await _dbConnection.QuerySingleOrDefaultAsync<CatastropheDto>(
            sql,
            new { gameSessionId = query.GameSessionId }
        );

        if (catastropheDTO is null)
        {
            return Result<CatastropheDto>.NotFound();
        }

        return Result<CatastropheDto>.Success(catastropheDTO);
    }
}
