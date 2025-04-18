using Bunker.Game.Domain.AggregateModels.Characters.Cards.CardActions;
using Bunker.Game.Infrastructure.Http.GameComponents.Contracts;

namespace Bunker.Game.Infrastructure.Http.GameComponents.Converters;

public static class CardActionConverter
{
    public static CardAction ToCardAction(this CardActionEntity entity)
    {
        return entity switch
        {
            AddCharacteristicEntity addCharacteristic => new AddCharacteristicCardAction(
                addCharacteristic.CharacteristicType.FromDto(),
                addCharacteristic.CharacteristicId,
                addCharacteristic.TargetCharactersCount
            ),
            SpyCharacteristicCardActionEntity spyCharacteristic => new SpyCharacteristicCardAction(
                spyCharacteristic.CharacteristicType.FromDto(),
                spyCharacteristic.TargetCharactersCount
            ),
            ExchangeCharacteristicActionEntity exchange => new ExchangeCharacteristicAction(
                exchange.CharacteristicType.FromDto()
            ),
            RerollCharacteristicCardActionEntity reroll => new RerollCharacteristicCardAction(
                reroll.CharacteristicType.FromDto(),
                reroll.IsSelfTarget,
                reroll.CharacteristicId,
                reroll.TargetCharactersCount
            ),
            RevealBunkerGameComponentCardActionEntity reveal => new RevealBunkerGameComponentCardAction(
                reveal.BunkerObjectType.FromDto()
            ),
            RemoveCharacteristicCardActionEntity remove => new RemoveCharacteristicCardAction(
                remove.CharacteristicType.FromDto(),
                remove.TargetCharactersCount
            ),
            RecreateCatastropheActionEntity recreateCatastrophe => new RecreateCatastropheAction(),
            RecreateBunkerActionEntity recreateBunker => new RecreateBunkerAction(),
            RecreateCharacterActionEntity recreateCharacter => new RecreateCharacterAction(
                recreateCharacter.TargetCharactersCount
            ),

            EmptyActionEntity _ => new EmptyAction(),
            _ => throw new InvalidOperationException("Unknown CardActionEntity type."),
        };
    }
}
