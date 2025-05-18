using System.Data;
using Ardalis.Result;
using Bunker.Application.Shared.CQRS;
using Bunker.Game.Application.Queries.Characters;
using Bunker.Game.Application.Queries.Characters.Models;
using Dapper;

namespace Bunker.Game.Infrastructure.Application.QueryHandlers;

public class GetCharactersByGameSessionQueryHandler
    : IQueryHandler<GetCharactersByGameSessionQuery, Result<IEnumerable<CharacterDto>>>
{
    private readonly IDbConnection _dbConnection;

    public GetCharactersByGameSessionQueryHandler(IDbConnection dbConnection)
    {
        _dbConnection = dbConnection;
    }

    public async Task<Result<IEnumerable<CharacterDto>>> Handle(
        GetCharactersByGameSessionQuery query,
        CancellationToken cancellation
    )
    {
        var sql =
            $@"
        SELECT 
            c.id AS {nameof(CharacterData.Id)},
            c.game_session_id AS {nameof(CharacterData.GameSessionId)},
            c.is_kicked AS {nameof(CharacterData.IsKicked)},
            c.additional_information_description AS {nameof(CharacterData.AdditionalInformation)},
            c.age_years AS {nameof(CharacterData.Age)},
            c.childbearing_can_give_birth AS {nameof(CharacterData.CanGiveBirth)},
            c.health_description AS {nameof(CharacterData.Health)},
            c.phobia_description AS {nameof(CharacterData.Phobia)},
            c.sex_description AS {nameof(CharacterData.Sex)},
            c.hobby_description AS {nameof(CharacterData.HobbyDescription)},
            c.hobby_experience AS {nameof(CharacterData.HobbyExperience)},
            c.profession_description AS {nameof(CharacterData.ProfessionDescription)},
            c.profession_experience_years AS {nameof(CharacterData.ProfessionExperience)},
            c.size_height AS {nameof(CharacterData.SizeHeight)},
            c.size_weight AS {nameof(CharacterData.SizeWeight)}
        FROM characters c
        WHERE c.game_session_id = @gameSessionId;
        
        SELECT 
            i.character_id AS {nameof(CharacterItemData.CharacterId)},
            i.id AS {nameof(CharacterItemData.Id)},
            i.description AS {nameof(CharacterItemData.Description)}
        FROM character_items i
        INNER JOIN characters c ON i.character_id = c.id
        WHERE c.game_session_id = @gameSessionId;
        
        SELECT 
            t.character_id AS {nameof(CharacterTraitData.CharacterId)},
            t.id AS {nameof(CharacterTraitData.Id)},
            t.description AS {nameof(CharacterTraitData.Description)}
        FROM character_traits t
        INNER JOIN characters c ON t.character_id = c.id
        WHERE c.game_session_id = @gameSessionId;
        
        SELECT 
            card.character_id AS {nameof(CharacterCardData.CharacterId)},
            card.id AS {nameof(CharacterCardData.Id)},
            card.description AS {nameof(CharacterCardData.Description)},
            card.is_activated AS {nameof(CharacterCardData.IsActivated)}
        FROM character_cards card
        INNER JOIN characters c ON card.character_id = c.id
        WHERE c.game_session_id = @gameSessionId;";

        var characterDict = new Dictionary<Guid, CharacterDto>();

        using var multi = await _dbConnection.QueryMultipleAsync(sql, new { gameSessionId = query.GameSessionId });

        var characters = await multi.ReadAsync<CharacterData>();

        if (!characters.Any())
        {
            return Result<IEnumerable<CharacterDto>>.NotFound();
        }

        foreach (var c in characters)
        {
            var character = new CharacterDto
            {
                Id = c.Id,
                GameSessionId = c.GameSessionId,
                IsKicked = c.IsKicked,
                AdditionalInformation = c.AdditionalInformation,
                Age = c.Age,
                CanGiveBirth = c.CanGiveBirth,
                Health = c.Health,
                Phobia = c.Phobia,
                Sex = c.Sex,
                Hobby = new HobbyDto { Description = c.HobbyDescription, Experience = c.HobbyExperience },
                Profession = new ProfessionDto
                {
                    Description = c.ProfessionDescription,
                    Experience = c.ProfessionExperience,
                },
                Size = new SizeDto { Height = c.SizeHeight, Weight = c.SizeWeight },
                Items = new List<CharacterItemDto>(),
                Traits = new List<TraitDto>(),
                Cards = new List<CardDto>(),
            };

            characterDict.Add(character.Id, character);
        }

        var items = await multi.ReadAsync<CharacterItemData>();
        foreach (var item in items)
        {
            if (characterDict.TryGetValue(item.CharacterId, out var characterItem))
            {
                characterItem.Items.Add(new CharacterItemDto { Description = item.Description });
            }
        }

        var traits = await multi.ReadAsync<CharacterTraitData>();
        foreach (var trait in traits)
        {
            if (characterDict.TryGetValue(trait.CharacterId, out var characterTrait))
            {
                characterTrait.Traits.Add(new TraitDto { Description = trait.Description });
            }
        }

        var cards = await multi.ReadAsync<CharacterCardData>();
        foreach (var card in cards)
        {
            if (characterDict.TryGetValue(card.CharacterId, out var characterCard))
            {
                characterCard.Cards.Add(
                    new CardDto
                    {
                        Id = card.Id,
                        Description = card.Description,
                        IsActivated = card.IsActivated,
                    }
                );
            }
        }

        return Result<IEnumerable<CharacterDto>>.Success(characterDict.Values);
    }

    private class CharacterData
    {
        public Guid Id { get; set; }
        public Guid GameSessionId { get; set; }
        public bool IsKicked { get; set; }
        public string AdditionalInformation { get; set; } = string.Empty;
        public int Age { get; set; }
        public bool CanGiveBirth { get; set; }
        public string Health { get; set; } = string.Empty;
        public string Phobia { get; set; } = string.Empty;
        public string Sex { get; set; } = string.Empty;
        public string HobbyDescription { get; set; } = string.Empty;
        public byte HobbyExperience { get; set; }
        public string ProfessionDescription { get; set; } = string.Empty;
        public byte ProfessionExperience { get; set; }
        public double SizeHeight { get; set; }
        public double SizeWeight { get; set; }
    }

    private class CharacterItemData
    {
        public Guid CharacterId { get; set; }
        public int Id { get; set; }
        public string Description { get; set; } = string.Empty;
    }

    private class CharacterTraitData
    {
        public Guid CharacterId { get; set; }
        public int Id { get; set; }
        public string Description { get; set; } = string.Empty;
    }

    private class CharacterCardData
    {
        public Guid CharacterId { get; set; }
        public Guid Id { get; set; }
        public string Description { get; set; } = string.Empty;
        public bool IsActivated { get; set; }
    }
}
