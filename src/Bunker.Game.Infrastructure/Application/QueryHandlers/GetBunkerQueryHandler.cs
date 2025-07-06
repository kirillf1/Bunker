using System.Data;
using Ardalis.Result;
using Bunker.Game.Application.Queries.Bunkers;
using Dapper;

namespace Bunker.Game.Infrastructure.Application.QueryHandlers;

public class GetBunkerQueryHandler : IQueryHandler<GetBunkerQuery, Result<BunkerDto>>
{
    private readonly IDbConnection _dbConnection;

    public GetBunkerQueryHandler(IDbConnection dbConnection)
    {
        _dbConnection = dbConnection;
    }

    public async Task<Result<BunkerDto>> Handle(GetBunkerQuery query, CancellationToken cancellation)
    {
        var sql =
            $@"
            SELECT 
                id AS {nameof(BunkerDto.Id)},
                game_session_id AS {nameof(BunkerDto.GameSessionId)},
                description AS {nameof(BunkerDto.Description)}
            FROM bunkers
            WHERE game_session_id = @id;

            SELECT 
                bunker_id,
                description AS {nameof(RoomDto.Description)},
                is_hidden AS {nameof(RoomDto.IsHidden)}
            FROM bunker_rooms
            WHERE bunker_id IN (SELECT id FROM bunkers WHERE game_session_id = @id);

            SELECT 
                bunker_id,
                description AS {nameof(BunkerItemDto.Description)},
                is_hidden AS {nameof(BunkerItemDto.IsHidden)}
            FROM bunker_items
            WHERE bunker_id IN (SELECT id FROM bunkers WHERE game_session_id = @id);

            SELECT 
                bunker_id,
                description AS {nameof(EnvironmentDto.Description)},
                is_hidden AS {nameof(EnvironmentDto.IsHidden)}
            FROM bunker_environments
            WHERE bunker_id IN (SELECT id FROM bunkers WHERE game_session_id = @id);
            ";
        using var multi = await _dbConnection.QueryMultipleAsync(sql, new { id = query.GameSessionId });

        var bunker = await multi.ReadSingleOrDefaultAsync<BunkerDto>();
        if (bunker is null)
        {
            return Result.NotFound();
        }

        var rooms = (await multi.ReadAsync<RoomDto>()).ToList();
        var items = (await multi.ReadAsync<BunkerItemDto>()).ToList();
        var environments = (await multi.ReadAsync<EnvironmentDto>()).ToList();

        bunker.Rooms = rooms;
        bunker.Items = items;
        bunker.Environments = environments;

        return bunker;
    }
}
