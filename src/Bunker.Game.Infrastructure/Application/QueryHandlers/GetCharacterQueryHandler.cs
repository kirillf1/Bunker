using System.Data;
using Ardalis.Result;
using Bunker.Game.Application.Queries.Characters;
using Bunker.Game.Application.Queries.Characters.Models;
using Dapper;

namespace Bunker.Game.Infrastructure.Application.QueryHandlers;

public class GetCharacterQueryHandler : IQueryHandler<GetCharacterQuery, Result<CharacterDto>>
{
    private readonly IDbConnection _dbConnection;

    public GetCharacterQueryHandler(IDbConnection dbConnection)
    {
        _dbConnection = dbConnection;
    }

    public async Task<Result<CharacterDto>> Handle(GetCharacterQuery query, CancellationToken cancellation)
    {
        var sql =
            $@"
    SELECT 
        id AS {nameof(CharacterDto.Id)},
        game_session_id AS {nameof(CharacterDto.GameSessionId)},
        is_kicked AS {nameof(CharacterDto.IsKicked)},
        additional_information_description AS {nameof(CharacterDto.AdditionalInformation)},
        age_years AS {nameof(CharacterDto.Age)},
        childbearing_can_give_birth AS {nameof(CharacterDto.CanGiveBirth)},
        health_description AS {nameof(CharacterDto.Health)},
        phobia_description AS {nameof(CharacterDto.Phobia)},
        sex_description AS {nameof(CharacterDto.Sex)}
    FROM characters
    WHERE id = @id;

    SELECT 
        hobby_description AS {nameof(HobbyDto.Description)},
        hobby_experience AS {nameof(HobbyDto.Experience)}
    FROM characters
    WHERE id = @id;

    SELECT 
        profession_description AS {nameof(ProfessionDto.Description)},
        profession_experience_years AS {nameof(ProfessionDto.Experience)}
    FROM characters
    WHERE id = @id;

    SELECT 
        size_height AS {nameof(SizeDto.Height)},
        size_weight AS {nameof(SizeDto.Weight)}
    FROM characters
    WHERE id = @id;

    SELECT 
        description AS {nameof(CharacterItemDto.Description)}
    FROM character_items
    WHERE character_id = @id;

    SELECT 
        description AS {nameof(TraitDto.Description)}
    FROM character_traits
    WHERE character_id = @id;

    SELECT 
        id AS {nameof(CardDto.Id)},
        description AS {nameof(CardDto.Description)},
        is_activated AS {nameof(CardDto.IsActivated)}
    FROM character_cards
    WHERE character_id = @id;
";

        using var multi = await _dbConnection.QueryMultipleAsync(sql, new { id = query.CharacterId });

        var character = await multi.ReadSingleOrDefaultAsync<CharacterDto>();
        if (character is null)
        {
            return Result.NotFound();
        }

        var hobby = await multi.ReadSingleAsync<HobbyDto>();
        var profession = await multi.ReadSingleAsync<ProfessionDto>();
        var size = await multi.ReadSingleAsync<SizeDto>();
        var items = (await multi.ReadAsync<CharacterItemDto>()).ToList();
        var traits = (await multi.ReadAsync<TraitDto>()).ToList();
        var cards = (await multi.ReadAsync<CardDto>()).ToList();

        character.Hobby = hobby;
        character.Profession = profession;
        character.Size = size;
        character.Items = items;
        character.Traits = traits;
        character.Cards = cards;

        return Result.Success(character);
    }
}
