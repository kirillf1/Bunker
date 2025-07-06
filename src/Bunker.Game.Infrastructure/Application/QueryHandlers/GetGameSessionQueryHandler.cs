using System.Data;
using Ardalis.Result;
using Bunker.Game.Application.Queries.GameSessions;
using Bunker.Game.Domain.AggregateModels.GameSessions;
using Dapper;

namespace Bunker.Game.Infrastructure.Application.QueryHandlers;

public class GetGameSessionQueryHandler : IQueryHandler<GetGameSessionQuery, Result<GameSessionDto>>
{
    private readonly IDbConnection _dbConnection;

    public GetGameSessionQueryHandler(IDbConnection dbConnection)
    {
        _dbConnection = dbConnection;
    }

    public async Task<Result<GameSessionDto>> Handle(GetGameSessionQuery query, CancellationToken cancellation)
    {
        var sql =
            $@"
        SELECT 
            gs.id AS {nameof(GameSessionData.Id)},
            gs.name AS {nameof(GameSessionData.Name)},
            gs.game_state AS {nameof(GameSessionData.GameState)},
            gs.free_seats_count AS {nameof(GameSessionData.FreeSeatsCount)},
            gs.game_result_description AS {nameof(GameSessionData.GameResultDescription)},
            gsc.id AS {nameof(GameSessionCharacterData.Id)},
            gsc.player_id AS {nameof(GameSessionCharacterData.PlayerId)},
            gsc.player_name AS {nameof(GameSessionCharacterData.PlayerName)},
            gsc.is_game_creator AS {nameof(GameSessionCharacterData.IsGameCreator)},
            gsc.is_kicked AS {nameof(GameSessionCharacterData.IsKicked)}
        FROM 
            game_sessions gs
        LEFT JOIN 
            game_session_characters gsc ON gs.id = gsc.game_session_id
        WHERE 
            gs.id = @gameSessionId";

        var lookup = new Dictionary<Guid, GameSessionDto>();

        var result = await _dbConnection.QueryAsync<GameSessionData, GameSessionCharacterData, GameSessionDto>(
            sql,
            (session, character) =>
            {
                if (!lookup.TryGetValue(session.Id, out var gameSessionDto))
                {
                    gameSessionDto = new GameSessionDto
                    {
                        Id = session.Id,
                        Name = session.Name,
                        GameState = Enum.Parse<GameState>(session.GameState),
                        FreeSeatsCount = session.FreeSeatsCount,
                        GameResultDescription = session.GameResultDescription,
                        Characters = new List<GameSessionCharacterDto>(),
                    };
                    lookup.Add(gameSessionDto.Id, gameSessionDto);
                }

                if (character?.Id is not null)
                {
                    var characterDto = new GameSessionCharacterDto
                    {
                        Id = character.Id,
                        PlayerId = character.PlayerId,
                        PlayerName = character.PlayerName,
                        IsOccupiedByPlayer = !string.IsNullOrEmpty(character.PlayerId),
                        IsGameCreator = character.IsGameCreator,
                        IsKicked = character.IsKicked,
                    };

                    gameSessionDto.Characters.Add(characterDto);
                }

                return gameSessionDto;
            },
            splitOn: nameof(GameSessionCharacterData.Id),
            param: new { gameSessionId = query.GameSessionId }
        );

        var gameSession = result.FirstOrDefault();

        if (gameSession == null)
        {
            return Result<GameSessionDto>.NotFound("Game session not found");
        }

        return Result<GameSessionDto>.Success(gameSession);
    }

    private class GameSessionData
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string GameState { get; set; } = string.Empty;
        public int FreeSeatsCount { get; set; }
        public string? GameResultDescription { get; set; }
    }

    private class GameSessionCharacterData
    {
        public Guid Id { get; set; }
        public string? PlayerId { get; set; }
        public string? PlayerName { get; set; }
        public bool IsGameCreator { get; set; }
        public bool IsKicked { get; set; }
    }
}
