using System.Text.Json.Serialization;
using Bunker.GameComponents.API.Entities.CharacterComponents.Cards.CardActions;

namespace Bunker.GameComponents.API.Models.CharacterComponents.Cards.CardActions;

[JsonPolymorphic(TypeDiscriminatorPropertyName = "$type")]
[JsonDerivedType(typeof(AddCharacteristicDto), "$AddCharacteristic")]
[JsonDerivedType(typeof(EmptyActionDto), "$EmptyAction")]
[JsonDerivedType(typeof(ExchangeCharacteristicActionDto), "$ExchangeCharacteristicAction")]
[JsonDerivedType(typeof(RecreateBunkerActionDto), "$RecreateBunkerAction")]
[JsonDerivedType(typeof(RecreateCatastropheActionDto), "$RecreateCatastropheAction")]
[JsonDerivedType(typeof(RecreateCharacterActionDto), "$RecreateCharacterAction")]
[JsonDerivedType(typeof(RemoveCharacteristicCardActionDto), "$RemoveCharacteristicCardAction")]
[JsonDerivedType(typeof(RerollCharacteristicCardActionDto), "$RerollCharacteristicCardAction")]
[JsonDerivedType(typeof(RevealBunkerGameComponentCardActionDto), "$RevealBunkerGameComponentCardAction")]
[JsonDerivedType(typeof(SpyCharacteristicCardActionDto), "$SpyCharacteristicCardAction")]
public abstract class CardActionDto
{
    public Guid Id { get; set; }

    public static CardActionDto MapToCardActionDto(CardActionEntity entity) =>
        entity switch
        {
            AddCharacteristicEntity e => new AddCharacteristicDto
            {
                Id = e.Id,
                CharacteristicType = e.CharacteristicType,
                CharacteristicId = e.CharacteristicId,
                TargetCharactersCount = e.TargetCharactersCount,
            },
            EmptyActionEntity e => new EmptyActionDto { Id = e.Id },
            ExchangeCharacteristicActionEntity e => new ExchangeCharacteristicActionDto
            {
                Id = e.Id,
                CharacteristicType = e.CharacteristicType,
            },
            RecreateBunkerActionEntity e => new RecreateBunkerActionDto { Id = e.Id },
            RecreateCatastropheActionEntity e => new RecreateCatastropheActionDto { Id = e.Id },
            RecreateCharacterActionEntity e => new RecreateCharacterActionDto
            {
                Id = e.Id,
                TargetCharactersCount = e.TargetCharactersCount,
            },
            RemoveCharacteristicCardActionEntity e => new RemoveCharacteristicCardActionDto
            {
                Id = e.Id,
                CharacteristicType = e.CharacteristicType,
                TargetCharactersCount = e.TargetCharactersCount,
            },
            RerollCharacteristicCardActionEntity e => new RerollCharacteristicCardActionDto
            {
                Id = e.Id,
                CharacteristicType = e.CharacteristicType,
                IsSelfTarget = e.IsSelfTarget,
                CharacteristicId = e.CharacteristicId,
                TargetCharactersCount = e.TargetCharactersCount,
            },
            RevealBunkerGameComponentCardActionEntity e => new RevealBunkerGameComponentCardActionDto
            {
                Id = e.Id,
                BunkerObjectType = e.BunkerObjectType,
            },
            SpyCharacteristicCardActionEntity e => new SpyCharacteristicCardActionDto
            {
                Id = e.Id,
                CharacteristicType = e.CharacteristicType,
                TargetCharactersCount = e.TargetCharactersCount,
            },
            _ => throw new NotSupportedException($"Unknown CardActionEntity type: {entity.GetType().Name}"),
        };

    public CardActionEntity MapToCardActionEntity() =>
        this switch
        {
            AddCharacteristicDto d => new AddCharacteristicEntity(
                d.CharacteristicType,
                d.CharacteristicId,
                d.TargetCharactersCount
            ),
            EmptyActionDto => new EmptyActionEntity(),
            ExchangeCharacteristicActionDto d => new ExchangeCharacteristicActionEntity(d.CharacteristicType),
            RecreateBunkerActionDto => new RecreateBunkerActionEntity(),
            RecreateCatastropheActionDto => new RecreateCatastropheActionEntity(),
            RecreateCharacterActionDto d => new RecreateCharacterActionEntity(d.TargetCharactersCount),
            RemoveCharacteristicCardActionDto d => new RemoveCharacteristicCardActionEntity(
                d.CharacteristicType,
                d.TargetCharactersCount
            ),
            RerollCharacteristicCardActionDto d => new RerollCharacteristicCardActionEntity(
                d.CharacteristicType,
                d.IsSelfTarget,
                d.CharacteristicId,
                d.TargetCharactersCount
            ),
            RevealBunkerGameComponentCardActionDto d => new RevealBunkerGameComponentCardActionEntity(
                d.BunkerObjectType
            ),
            SpyCharacteristicCardActionDto d => new SpyCharacteristicCardActionEntity(
                d.CharacteristicType,
                d.TargetCharactersCount
            ),
            _ => throw new NotSupportedException($"Unknown CardActionDto type: {this.GetType().Name}"),
        };
}
