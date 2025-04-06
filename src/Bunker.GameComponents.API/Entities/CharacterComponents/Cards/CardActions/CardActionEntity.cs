using System.Text.Json.Serialization;

namespace Bunker.GameComponents.API.Entities.CharacterComponents.Cards.CardActions;

[JsonPolymorphic(TypeDiscriminatorPropertyName = "$type")]
[JsonDerivedType(typeof(AddCharacteristicEntity), "$AddCharacteristic")]
[JsonDerivedType(typeof(EmptyActionEntity), "$EmptyAction")]
[JsonDerivedType(typeof(ExchangeCharacteristicActionEntity), "$ExchangeCharacteristicAction")]
[JsonDerivedType(typeof(RecreateBunkerActionEntity), "$RecreateBunkerAction")]
[JsonDerivedType(typeof(RecreateCatastropheActionEntity), "$RecreateCatastropheAction")]
[JsonDerivedType(typeof(RecreateCharacterActionEntity), "$RecreateCharacterAction")]
[JsonDerivedType(typeof(RemoveCharacteristicCardActionEntity), "$RemoveCharacteristicCardAction")]
[JsonDerivedType(typeof(RerollCharacteristicCardActionEntity), "$RerollCharacteristicCardAction")]
[JsonDerivedType(typeof(RevealBunkerGameComponentCardActionEntity), "$RevealBunkerGameComponentCardAction")]
[JsonDerivedType(typeof(SpyCharacteristicCardActionEntity), "$SpyCharacteristicCardAction")]
public class CardActionEntity
{
    protected CardActionEntity() { }
}
