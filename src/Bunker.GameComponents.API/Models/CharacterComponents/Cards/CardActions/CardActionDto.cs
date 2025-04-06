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

    // TODO change by Template method pattern
    public CardActionEntity MapToCardActionEntity()
    {
        var cardAction = this;

        return cardAction switch
        {
            AddCharacteristicDto d => new AddCharacteristicEntity(
                d.CharacteristicType,
                d.CharacteristicId,
                d.TargetCharactersCount
            )
            {
                Id = cardAction.Id,
            },
            EmptyActionDto => new EmptyActionEntity() { Id = cardAction.Id },
            ExchangeCharacteristicActionDto d => new ExchangeCharacteristicActionEntity(d.CharacteristicType)
            {
                Id = cardAction.Id,
            },
            RecreateBunkerActionDto => new RecreateBunkerActionEntity() { Id = cardAction.Id },
            RecreateCatastropheActionDto => new RecreateCatastropheActionEntity() { Id = cardAction.Id },
            RecreateCharacterActionDto d => new RecreateCharacterActionEntity(d.TargetCharactersCount)
            {
                Id = cardAction.Id,
            },
            RemoveCharacteristicCardActionDto d => new RemoveCharacteristicCardActionEntity(
                d.CharacteristicType,
                d.TargetCharactersCount
            )
            {
                Id = cardAction.Id,
            },
            RerollCharacteristicCardActionDto d => new RerollCharacteristicCardActionEntity(
                d.CharacteristicType,
                d.IsSelfTarget,
                d.CharacteristicId,
                d.TargetCharactersCount
            )
            {
                Id = cardAction.Id,
            },
            RevealBunkerGameComponentCardActionDto d => new RevealBunkerGameComponentCardActionEntity(
                d.BunkerObjectType
            )
            {
                Id = cardAction.Id,
            },
            SpyCharacteristicCardActionDto d => new SpyCharacteristicCardActionEntity(
                d.CharacteristicType,
                d.TargetCharactersCount
            )
            {
                Id = cardAction.Id,
            },
            _ => throw new NotSupportedException($"Unknown CardActionDto type: {this.GetType().Name}"),
        };
    }
}
