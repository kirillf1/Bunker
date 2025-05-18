using System.Data;
using System.Text.Json;
using Ardalis.Result;
using Bunker.Game.Application.Queries.Characters;
using Bunker.Game.Domain.AggregateModels.Characters.Cards;
using Bunker.Game.Domain.AggregateModels.Characters.Cards.CardActions;
using Bunker.Game.Infrastructure.Data.Converters;
using Dapper;

namespace Bunker.Game.Infrastructure.Application.QueryHandlers;

public class GetCardRequirementsQueryHandler : IQueryHandler<GetCardRequirementsQuery, Result<CardActionRequirements>>
{
    private readonly IDbConnection _dbConnection;
    private readonly JsonSerializerOptions _jsonOptions;

    public GetCardRequirementsQueryHandler(IDbConnection dbConnection)
    {
        _dbConnection = dbConnection;
        _jsonOptions = new JsonSerializerOptions();
        _jsonOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter());
        _jsonOptions.Converters.Add(new CardActionJsonConverter());
    }

    public async Task<Result<CardActionRequirements>> Handle(
        GetCardRequirementsQuery query,
        CancellationToken cancellation
    )
    {
        var sql =
            @"
            SELECT card_action
            FROM character_cards
            WHERE character_id = @characterId AND id = @cardId;
        ";

        var cardActionJson = await _dbConnection.QuerySingleOrDefaultAsync<string>(
            sql,
            new { characterId = query.CharacterId, cardId = query.CardId }
        );

        if (string.IsNullOrEmpty(cardActionJson))
        {
            return Result.NotFound();
        }

        var cardAction = JsonSerializer.Deserialize<CardAction>(cardActionJson, _jsonOptions);
        if (cardAction is null)
        {
            return Result.Error("Failed deserialize card action");
        }

        var requirements = cardAction.GetCurrentCardActionRequirements();
        return Result.Success(requirements);
    }
}
