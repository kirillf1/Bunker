//----------------------
// <auto-generated>
//     Generated using the NSwag toolchain v14.3.0.0 (NJsonSchema v11.2.0.0 (Newtonsoft.Json v13.0.0.0)) (http://NSwag.org)
// </auto-generated>
//----------------------

#pragma warning disable 108 // Disable "CS0108 '{derivedDto}.ToJson()' hides inherited member '{dtoBase}.ToJson()'. Use the new keyword if hiding was intended."
#pragma warning disable 114 // Disable "CS0114 '{derivedDto}.RaisePropertyChanged(String)' hides inherited member 'dtoBase.RaisePropertyChanged(String)'. To make the current member override that implementation, add the override keyword. Otherwise add the new keyword."
#pragma warning disable 472 // Disable "CS0472 The result of the expression is always 'false' since a value of type 'Int32' is never equal to 'null' of type 'Int32?'
#pragma warning disable 612 // Disable "CS0612 '...' is obsolete"
#pragma warning disable 649 // Disable "CS0649 Field is never assigned to, and will always have its default value null"
#pragma warning disable 1573 // Disable "CS1573 Parameter '...' has no matching param tag in the XML comment for ...
#pragma warning disable 1591 // Disable "CS1591 Missing XML comment for publicly visible type or member ..."
#pragma warning disable 8073 // Disable "CS8073 The result of the expression is always 'false' since a value of type 'T' is never equal to 'null' of type 'T?'"
#pragma warning disable 3016 // Disable "CS3016 Arrays as attribute arguments is not CLS-compliant"
#pragma warning disable 8600 // Disable "CS8600 Converting null literal or possible null value to non-nullable type"
#pragma warning disable 8602 // Disable "CS8602 Dereference of a possibly null reference"
#pragma warning disable 8603 // Disable "CS8603 Possible null reference return"
#pragma warning disable 8604 // Disable "CS8604 Possible null reference argument for parameter"
#pragma warning disable 8625 // Disable "CS8625 Cannot convert null literal to non-nullable reference type"
#pragma warning disable 8765 // Disable "CS8765 Nullability of type of parameter doesn't match overridden member (possibly because of nullability attributes)."

namespace Bunker.Game.Infrastructure.Http.GameComponents.Contracts
{
    using System = global::System;

    

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "14.3.0.0 (NJsonSchema v11.2.0.0 (Newtonsoft.Json v13.0.0.0))")]
    public partial class AddCharacteristicEntity : CardActionEntity
    {

        [System.Text.Json.Serialization.JsonPropertyName("characteristicType")]
        [System.Text.Json.Serialization.JsonConverter(typeof(System.Text.Json.Serialization.JsonStringEnumConverter))]
        public CharacteristicType CharacteristicType { get; set; }

        [System.Text.Json.Serialization.JsonPropertyName("characteristicId")]
        public System.Guid? CharacteristicId { get; set; }

        [System.Text.Json.Serialization.JsonPropertyName("targetCharactersCount")]
        public int TargetCharactersCount { get; set; }

    }

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "14.3.0.0 (NJsonSchema v11.2.0.0 (Newtonsoft.Json v13.0.0.0))")]
    public partial class AdditionalInformationDto
    {

        [System.Text.Json.Serialization.JsonPropertyName("id")]
        public System.Guid Id { get; set; }

        [System.Text.Json.Serialization.JsonPropertyName("description")]
        public string Description { get; set; }

    }

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "14.3.0.0 (NJsonSchema v11.2.0.0 (Newtonsoft.Json v13.0.0.0))")]
    public partial class BunkerDescriptionDto
    {

        [System.Text.Json.Serialization.JsonPropertyName("id")]
        public System.Guid Id { get; set; }

        [System.Text.Json.Serialization.JsonPropertyName("text")]
        public string Text { get; set; }

    }

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "14.3.0.0 (NJsonSchema v11.2.0.0 (Newtonsoft.Json v13.0.0.0))")]
    public partial class BunkerItemDto
    {

        [System.Text.Json.Serialization.JsonPropertyName("id")]
        public System.Guid Id { get; set; }

        [System.Text.Json.Serialization.JsonPropertyName("description")]
        public string Description { get; set; }

    }

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "14.3.0.0 (NJsonSchema v11.2.0.0 (Newtonsoft.Json v13.0.0.0))")]
    public enum BunkerObjectType
    {

        [System.Runtime.Serialization.EnumMember(Value = @"BunkerRoom")]
        BunkerRoom = 0,

        [System.Runtime.Serialization.EnumMember(Value = @"BunkerEnvironment")]
        BunkerEnvironment = 1,

        [System.Runtime.Serialization.EnumMember(Value = @"BunkerItem")]
        BunkerItem = 2,

    }

    [JsonInheritanceConverter(typeof(CardActionEntity), "$type")]
    [JsonInheritanceAttribute("$AddCharacteristic", typeof(AddCharacteristicEntity))]
    [JsonInheritanceAttribute("$EmptyAction", typeof(EmptyActionEntity))]
    [JsonInheritanceAttribute("$ExchangeCharacteristicAction", typeof(ExchangeCharacteristicActionEntity))]
    [JsonInheritanceAttribute("$RecreateBunkerAction", typeof(RecreateBunkerActionEntity))]
    [JsonInheritanceAttribute("$RecreateCatastropheAction", typeof(RecreateCatastropheActionEntity))]
    [JsonInheritanceAttribute("$RecreateCharacterAction", typeof(RecreateCharacterActionEntity))]
    [JsonInheritanceAttribute("$RemoveCharacteristicCardAction", typeof(RemoveCharacteristicCardActionEntity))]
    [JsonInheritanceAttribute("$RerollCharacteristicCardAction", typeof(RerollCharacteristicCardActionEntity))]
    [JsonInheritanceAttribute("$RevealBunkerGameComponentCardAction", typeof(RevealBunkerGameComponentCardActionEntity))]
    [JsonInheritanceAttribute("$SpyCharacteristicCardAction", typeof(SpyCharacteristicCardActionEntity))]
    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "14.3.0.0 (NJsonSchema v11.2.0.0 (Newtonsoft.Json v13.0.0.0))")]
    public partial class CardActionEntity
    {

    }

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "14.3.0.0 (NJsonSchema v11.2.0.0 (Newtonsoft.Json v13.0.0.0))")]
    public partial class CardCreateDto
    {

        [System.Text.Json.Serialization.JsonPropertyName("description")]
        public string Description { get; set; }

        [System.Text.Json.Serialization.JsonPropertyName("cardAction")]
        public CardActionEntity CardAction { get; set; }

    }

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "14.3.0.0 (NJsonSchema v11.2.0.0 (Newtonsoft.Json v13.0.0.0))")]
    public partial class CardDto
    {

        [System.Text.Json.Serialization.JsonPropertyName("id")]
        public System.Guid Id { get; set; }

        [System.Text.Json.Serialization.JsonPropertyName("description")]
        public string Description { get; set; }

        [System.Text.Json.Serialization.JsonPropertyName("cardAction")]
        public CardActionEntity CardAction { get; set; }

    }

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "14.3.0.0 (NJsonSchema v11.2.0.0 (Newtonsoft.Json v13.0.0.0))")]
    public partial class CardUpdateDto
    {

        [System.Text.Json.Serialization.JsonPropertyName("description")]
        public string Description { get; set; }

        [System.Text.Json.Serialization.JsonPropertyName("cardAction")]
        public CardActionEntity CardAction { get; set; }

    }

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "14.3.0.0 (NJsonSchema v11.2.0.0 (Newtonsoft.Json v13.0.0.0))")]
    public partial class CatastropheDto
    {

        [System.Text.Json.Serialization.JsonPropertyName("id")]
        public System.Guid Id { get; set; }

        [System.Text.Json.Serialization.JsonPropertyName("description")]
        public string Description { get; set; }

    }

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "14.3.0.0 (NJsonSchema v11.2.0.0 (Newtonsoft.Json v13.0.0.0))")]
    public enum CharacteristicType
    {

        [System.Runtime.Serialization.EnumMember(Value = @"Phobia")]
        Phobia = 0,

        [System.Runtime.Serialization.EnumMember(Value = @"Hobby")]
        Hobby = 1,

        [System.Runtime.Serialization.EnumMember(Value = @"AdditionalInformation")]
        AdditionalInformation = 2,

        [System.Runtime.Serialization.EnumMember(Value = @"Health")]
        Health = 3,

        [System.Runtime.Serialization.EnumMember(Value = @"CharacterItem")]
        CharacterItem = 4,

        [System.Runtime.Serialization.EnumMember(Value = @"Profession")]
        Profession = 5,

        [System.Runtime.Serialization.EnumMember(Value = @"Trait")]
        Trait = 6,

        [System.Runtime.Serialization.EnumMember(Value = @"Card")]
        Card = 7,

    }

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "14.3.0.0 (NJsonSchema v11.2.0.0 (Newtonsoft.Json v13.0.0.0))")]
    public partial class CreateAdditionalInformationDto
    {

        [System.Text.Json.Serialization.JsonPropertyName("description")]
        public string Description { get; set; }

    }

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "14.3.0.0 (NJsonSchema v11.2.0.0 (Newtonsoft.Json v13.0.0.0))")]
    public partial class CreateBunkerDescriptionDto
    {

        [System.Text.Json.Serialization.JsonPropertyName("text")]
        public string Text { get; set; }

    }

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "14.3.0.0 (NJsonSchema v11.2.0.0 (Newtonsoft.Json v13.0.0.0))")]
    public partial class CreateBunkerItemDto
    {

        [System.Text.Json.Serialization.JsonPropertyName("description")]
        public string Description { get; set; }

    }

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "14.3.0.0 (NJsonSchema v11.2.0.0 (Newtonsoft.Json v13.0.0.0))")]
    public partial class CreateCatastropheDto
    {

        [System.Text.Json.Serialization.JsonPropertyName("description")]
        public string Description { get; set; }

    }

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "14.3.0.0 (NJsonSchema v11.2.0.0 (Newtonsoft.Json v13.0.0.0))")]
    public partial class CreateEnvironmentDto
    {

        [System.Text.Json.Serialization.JsonPropertyName("description")]
        public string Description { get; set; }

    }

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "14.3.0.0 (NJsonSchema v11.2.0.0 (Newtonsoft.Json v13.0.0.0))")]
    public partial class CreateHobbyDto
    {

        [System.Text.Json.Serialization.JsonPropertyName("description")]
        public string Description { get; set; }

    }

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "14.3.0.0 (NJsonSchema v11.2.0.0 (Newtonsoft.Json v13.0.0.0))")]
    public partial class CreateItemDto
    {

        [System.Text.Json.Serialization.JsonPropertyName("description")]
        public string Description { get; set; }

    }

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "14.3.0.0 (NJsonSchema v11.2.0.0 (Newtonsoft.Json v13.0.0.0))")]
    public partial class CreatePhobiaDto
    {

        [System.Text.Json.Serialization.JsonPropertyName("description")]
        public string Description { get; set; }

    }

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "14.3.0.0 (NJsonSchema v11.2.0.0 (Newtonsoft.Json v13.0.0.0))")]
    public partial class CreateProfessionDto
    {

        [System.Text.Json.Serialization.JsonPropertyName("description")]
        public string Description { get; set; }

    }

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "14.3.0.0 (NJsonSchema v11.2.0.0 (Newtonsoft.Json v13.0.0.0))")]
    public partial class CreateRoomDto
    {

        [System.Text.Json.Serialization.JsonPropertyName("description")]
        public string Description { get; set; }

    }

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "14.3.0.0 (NJsonSchema v11.2.0.0 (Newtonsoft.Json v13.0.0.0))")]
    public partial class CreateTraitDto
    {

        [System.Text.Json.Serialization.JsonPropertyName("description")]
        public string Description { get; set; }

    }

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "14.3.0.0 (NJsonSchema v11.2.0.0 (Newtonsoft.Json v13.0.0.0))")]
    public partial class EmptyActionEntity : CardActionEntity
    {

    }

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "14.3.0.0 (NJsonSchema v11.2.0.0 (Newtonsoft.Json v13.0.0.0))")]
    public partial class EnvironmentDto
    {

        [System.Text.Json.Serialization.JsonPropertyName("id")]
        public System.Guid Id { get; set; }

        [System.Text.Json.Serialization.JsonPropertyName("description")]
        public string Description { get; set; }

    }

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "14.3.0.0 (NJsonSchema v11.2.0.0 (Newtonsoft.Json v13.0.0.0))")]
    public partial class ExchangeCharacteristicActionEntity : CardActionEntity
    {

        [System.Text.Json.Serialization.JsonPropertyName("characteristicType")]
        [System.Text.Json.Serialization.JsonConverter(typeof(System.Text.Json.Serialization.JsonStringEnumConverter))]
        public CharacteristicType CharacteristicType { get; set; }

    }

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "14.3.0.0 (NJsonSchema v11.2.0.0 (Newtonsoft.Json v13.0.0.0))")]
    public partial class HealthCreateDto
    {

        [System.Text.Json.Serialization.JsonPropertyName("description")]
        public string Description { get; set; }

    }

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "14.3.0.0 (NJsonSchema v11.2.0.0 (Newtonsoft.Json v13.0.0.0))")]
    public partial class HealthDto
    {

        [System.Text.Json.Serialization.JsonPropertyName("id")]
        public System.Guid Id { get; set; }

        [System.Text.Json.Serialization.JsonPropertyName("description")]
        public string Description { get; set; }

    }

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "14.3.0.0 (NJsonSchema v11.2.0.0 (Newtonsoft.Json v13.0.0.0))")]
    public partial class HealthUpdateDto
    {

        [System.Text.Json.Serialization.JsonPropertyName("description")]
        public string Description { get; set; }

    }

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "14.3.0.0 (NJsonSchema v11.2.0.0 (Newtonsoft.Json v13.0.0.0))")]
    public partial class HobbyDto
    {

        [System.Text.Json.Serialization.JsonPropertyName("id")]
        public System.Guid Id { get; set; }

        [System.Text.Json.Serialization.JsonPropertyName("description")]
        public string Description { get; set; }

    }

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "14.3.0.0 (NJsonSchema v11.2.0.0 (Newtonsoft.Json v13.0.0.0))")]
    public partial class HttpValidationProblemDetails : ProblemDetails
    {

        [System.Text.Json.Serialization.JsonPropertyName("errors")]
        public System.Collections.Generic.IDictionary<string, System.Collections.Generic.ICollection<string>> Errors { get; set; }

    }

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "14.3.0.0 (NJsonSchema v11.2.0.0 (Newtonsoft.Json v13.0.0.0))")]
    public partial class ItemDto
    {

        [System.Text.Json.Serialization.JsonPropertyName("id")]
        public System.Guid Id { get; set; }

        [System.Text.Json.Serialization.JsonPropertyName("description")]
        public string Description { get; set; }

    }

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "14.3.0.0 (NJsonSchema v11.2.0.0 (Newtonsoft.Json v13.0.0.0))")]
    public partial class PhobiaDto
    {

        [System.Text.Json.Serialization.JsonPropertyName("id")]
        public System.Guid Id { get; set; }

        [System.Text.Json.Serialization.JsonPropertyName("description")]
        public string Description { get; set; }

    }

    [JsonInheritanceConverter(typeof(ProblemDetails), "$type")]
    [JsonInheritanceAttribute("HttpValidationProblemDetails", typeof(HttpValidationProblemDetails))]
    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "14.3.0.0 (NJsonSchema v11.2.0.0 (Newtonsoft.Json v13.0.0.0))")]
    public partial class ProblemDetails
    {

        [System.Text.Json.Serialization.JsonPropertyName("type")]
        public string Type { get; set; }

        [System.Text.Json.Serialization.JsonPropertyName("title")]
        public string Title { get; set; }

        [System.Text.Json.Serialization.JsonPropertyName("status")]
        public int? Status { get; set; }

        [System.Text.Json.Serialization.JsonPropertyName("detail")]
        public string Detail { get; set; }

        [System.Text.Json.Serialization.JsonPropertyName("instance")]
        public string Instance { get; set; }

        private System.Collections.Generic.IDictionary<string, object> _additionalProperties;

        [System.Text.Json.Serialization.JsonExtensionData]
        public System.Collections.Generic.IDictionary<string, object> AdditionalProperties
        {
            get { return _additionalProperties ?? (_additionalProperties = new System.Collections.Generic.Dictionary<string, object>()); }
            set { _additionalProperties = value; }
        }

    }

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "14.3.0.0 (NJsonSchema v11.2.0.0 (Newtonsoft.Json v13.0.0.0))")]
    public partial class ProfessionDto
    {

        [System.Text.Json.Serialization.JsonPropertyName("id")]
        public System.Guid Id { get; set; }

        [System.Text.Json.Serialization.JsonPropertyName("description")]
        public string Description { get; set; }

    }

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "14.3.0.0 (NJsonSchema v11.2.0.0 (Newtonsoft.Json v13.0.0.0))")]
    public partial class RecreateBunkerActionEntity : CardActionEntity
    {

    }

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "14.3.0.0 (NJsonSchema v11.2.0.0 (Newtonsoft.Json v13.0.0.0))")]
    public partial class RecreateCatastropheActionEntity : CardActionEntity
    {

    }

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "14.3.0.0 (NJsonSchema v11.2.0.0 (Newtonsoft.Json v13.0.0.0))")]
    public partial class RecreateCharacterActionEntity : CardActionEntity
    {

        [System.Text.Json.Serialization.JsonPropertyName("targetCharactersCount")]
        public int TargetCharactersCount { get; set; }

    }

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "14.3.0.0 (NJsonSchema v11.2.0.0 (Newtonsoft.Json v13.0.0.0))")]
    public partial class RemoveCharacteristicCardActionEntity : CardActionEntity
    {

        [System.Text.Json.Serialization.JsonPropertyName("characteristicType")]
        [System.Text.Json.Serialization.JsonConverter(typeof(System.Text.Json.Serialization.JsonStringEnumConverter))]
        public CharacteristicType CharacteristicType { get; set; }

        [System.Text.Json.Serialization.JsonPropertyName("targetCharactersCount")]
        public int TargetCharactersCount { get; set; }

    }

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "14.3.0.0 (NJsonSchema v11.2.0.0 (Newtonsoft.Json v13.0.0.0))")]
    public partial class RerollCharacteristicCardActionEntity : CardActionEntity
    {

        [System.Text.Json.Serialization.JsonPropertyName("characteristicType")]
        [System.Text.Json.Serialization.JsonConverter(typeof(System.Text.Json.Serialization.JsonStringEnumConverter))]
        public CharacteristicType CharacteristicType { get; set; }

        [System.Text.Json.Serialization.JsonPropertyName("isSelfTarget")]
        public bool IsSelfTarget { get; set; }

        [System.Text.Json.Serialization.JsonPropertyName("characteristicId")]
        public System.Guid? CharacteristicId { get; set; }

        [System.Text.Json.Serialization.JsonPropertyName("targetCharactersCount")]
        public int TargetCharactersCount { get; set; }

    }

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "14.3.0.0 (NJsonSchema v11.2.0.0 (Newtonsoft.Json v13.0.0.0))")]
    public partial class RevealBunkerGameComponentCardActionEntity : CardActionEntity
    {

        [System.Text.Json.Serialization.JsonPropertyName("bunkerObjectType")]
        [System.Text.Json.Serialization.JsonConverter(typeof(System.Text.Json.Serialization.JsonStringEnumConverter))]
        public BunkerObjectType BunkerObjectType { get; set; }

    }

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "14.3.0.0 (NJsonSchema v11.2.0.0 (Newtonsoft.Json v13.0.0.0))")]
    public partial class RoomDto
    {

        [System.Text.Json.Serialization.JsonPropertyName("id")]
        public System.Guid Id { get; set; }

        [System.Text.Json.Serialization.JsonPropertyName("description")]
        public string Description { get; set; }

    }

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "14.3.0.0 (NJsonSchema v11.2.0.0 (Newtonsoft.Json v13.0.0.0))")]
    public partial class SpyCharacteristicCardActionEntity : CardActionEntity
    {

        [System.Text.Json.Serialization.JsonPropertyName("characteristicType")]
        [System.Text.Json.Serialization.JsonConverter(typeof(System.Text.Json.Serialization.JsonStringEnumConverter))]
        public CharacteristicType CharacteristicType { get; set; }

        [System.Text.Json.Serialization.JsonPropertyName("targetCharactersCount")]
        public int TargetCharactersCount { get; set; }

    }

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "14.3.0.0 (NJsonSchema v11.2.0.0 (Newtonsoft.Json v13.0.0.0))")]
    public partial class TraitDto
    {

        [System.Text.Json.Serialization.JsonPropertyName("id")]
        public System.Guid Id { get; set; }

        [System.Text.Json.Serialization.JsonPropertyName("description")]
        public string Description { get; set; }

    }

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "14.3.0.0 (NJsonSchema v11.2.0.0 (Newtonsoft.Json v13.0.0.0))")]
    public partial class UpdateAdditionalInformationDto
    {

        [System.Text.Json.Serialization.JsonPropertyName("description")]
        public string Description { get; set; }

    }

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "14.3.0.0 (NJsonSchema v11.2.0.0 (Newtonsoft.Json v13.0.0.0))")]
    public partial class UpdateBunkerDescriptionDto
    {

        [System.Text.Json.Serialization.JsonPropertyName("text")]
        public string Text { get; set; }

    }

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "14.3.0.0 (NJsonSchema v11.2.0.0 (Newtonsoft.Json v13.0.0.0))")]
    public partial class UpdateBunkerItemDto
    {

        [System.Text.Json.Serialization.JsonPropertyName("description")]
        public string Description { get; set; }

    }

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "14.3.0.0 (NJsonSchema v11.2.0.0 (Newtonsoft.Json v13.0.0.0))")]
    public partial class UpdateCatastropheDto
    {

        [System.Text.Json.Serialization.JsonPropertyName("description")]
        public string Description { get; set; }

    }

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "14.3.0.0 (NJsonSchema v11.2.0.0 (Newtonsoft.Json v13.0.0.0))")]
    public partial class UpdateEnvironmentDto
    {

        [System.Text.Json.Serialization.JsonPropertyName("description")]
        public string Description { get; set; }

    }

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "14.3.0.0 (NJsonSchema v11.2.0.0 (Newtonsoft.Json v13.0.0.0))")]
    public partial class UpdateHobbyDto
    {

        [System.Text.Json.Serialization.JsonPropertyName("description")]
        public string Description { get; set; }

    }

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "14.3.0.0 (NJsonSchema v11.2.0.0 (Newtonsoft.Json v13.0.0.0))")]
    public partial class UpdateItemDto
    {

        [System.Text.Json.Serialization.JsonPropertyName("description")]
        public string Description { get; set; }

    }

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "14.3.0.0 (NJsonSchema v11.2.0.0 (Newtonsoft.Json v13.0.0.0))")]
    public partial class UpdatePhobiaDto
    {

        [System.Text.Json.Serialization.JsonPropertyName("description")]
        public string Description { get; set; }

    }

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "14.3.0.0 (NJsonSchema v11.2.0.0 (Newtonsoft.Json v13.0.0.0))")]
    public partial class UpdateProfessionDto
    {

        [System.Text.Json.Serialization.JsonPropertyName("description")]
        public string Description { get; set; }

    }

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "14.3.0.0 (NJsonSchema v11.2.0.0 (Newtonsoft.Json v13.0.0.0))")]
    public partial class UpdateRoomDto
    {

        [System.Text.Json.Serialization.JsonPropertyName("description")]
        public string Description { get; set; }

    }

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "14.3.0.0 (NJsonSchema v11.2.0.0 (Newtonsoft.Json v13.0.0.0))")]
    public partial class UpdateTraitDto
    {

        [System.Text.Json.Serialization.JsonPropertyName("description")]
        public string Description { get; set; }

    }

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "14.3.0.0 (NJsonSchema v11.2.0.0 (Newtonsoft.Json v13.0.0.0))")]
    [System.AttributeUsage(System.AttributeTargets.Class | System.AttributeTargets.Interface, AllowMultiple = true)]
    internal class JsonInheritanceAttribute : System.Attribute
    {
        public JsonInheritanceAttribute(string key, System.Type type)
        {
            Key = key;
            Type = type;
        }

        public string Key { get; }

        public System.Type Type { get; }
    }

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "14.3.0.0 (NJsonSchema v11.2.0.0 (Newtonsoft.Json v13.0.0.0))")]
    internal class JsonInheritanceConverterAttribute : System.Text.Json.Serialization.JsonConverterAttribute
    {
        public string DiscriminatorName { get; }

        public JsonInheritanceConverterAttribute(System.Type baseType, string discriminatorName = "discriminator")
            : base(typeof(JsonInheritanceConverter<>).MakeGenericType(baseType))
        {
            DiscriminatorName = discriminatorName;
        }
    }

    public class JsonInheritanceConverter<TBase> : System.Text.Json.Serialization.JsonConverter<TBase>
    {
        private readonly string _discriminatorName;

        public JsonInheritanceConverter()
        {
            var attribute = System.Reflection.CustomAttributeExtensions.GetCustomAttribute<JsonInheritanceConverterAttribute>(typeof(TBase));
            _discriminatorName = attribute?.DiscriminatorName ?? "discriminator";
        }

        public JsonInheritanceConverter(string discriminatorName)
        {
            _discriminatorName = discriminatorName;
        }

        public string DiscriminatorName { get { return _discriminatorName; } }

        public override TBase Read(ref System.Text.Json.Utf8JsonReader reader, System.Type typeToConvert, System.Text.Json.JsonSerializerOptions options)
        {
            var document = System.Text.Json.JsonDocument.ParseValue(ref reader);
            var hasDiscriminator = document.RootElement.TryGetProperty(_discriminatorName, out var discriminator);
            var subtype = GetDiscriminatorType(document.RootElement, typeToConvert, hasDiscriminator ? discriminator.GetString() : null);

            var bufferWriter = new System.IO.MemoryStream();
            using (var writer = new System.Text.Json.Utf8JsonWriter(bufferWriter))
            {
                document.RootElement.WriteTo(writer);
            }

            return (TBase)System.Text.Json.JsonSerializer.Deserialize(bufferWriter.ToArray(), subtype, options);
        }

        public override void Write(System.Text.Json.Utf8JsonWriter writer, TBase value, System.Text.Json.JsonSerializerOptions options)
        {
            if (value != null)
            {
                writer.WriteStartObject();
                writer.WriteString(_discriminatorName, GetDiscriminatorValue(value.GetType()));

                var bytes = System.Text.Json.JsonSerializer.SerializeToUtf8Bytes((object)value, options);
                var document = System.Text.Json.JsonDocument.Parse(bytes);
                foreach (var property in document.RootElement.EnumerateObject())
                {
                    property.WriteTo(writer);
                }

                writer.WriteEndObject();
            }
            else
            {
                writer.WriteNullValue();
            }
        }

        public string GetDiscriminatorValue(System.Type type)
        {
            var jsonInheritanceAttributeDiscriminator = GetSubtypeDiscriminator(type);
            if (jsonInheritanceAttributeDiscriminator != null)
            {
                return jsonInheritanceAttributeDiscriminator;
            }

            return type.Name;
        }

        protected System.Type GetDiscriminatorType(System.Text.Json.JsonElement jObject, System.Type objectType, string discriminatorValue)
        {
            if (discriminatorValue != null)
            {
                var jsonInheritanceAttributeSubtype = GetObjectSubtype(objectType, discriminatorValue);
                if (jsonInheritanceAttributeSubtype != null)
                {
                    return jsonInheritanceAttributeSubtype;
                }

                if (objectType.Name == discriminatorValue)
                {
                    return objectType;
                }

                var typeName = objectType.Namespace + "." + discriminatorValue;
                var subtype = System.Reflection.IntrospectionExtensions.GetTypeInfo(objectType).Assembly.GetType(typeName);
                if (subtype != null)
                {
                    return subtype;
                }
            }

            throw new System.InvalidOperationException("Could not find subtype of '" + objectType.Name + "' with discriminator '" + discriminatorValue + "'.");
        }

        private System.Type GetObjectSubtype(System.Type baseType, string discriminatorValue)
        {
            foreach (var attribute in System.Reflection.CustomAttributeExtensions.GetCustomAttributes<JsonInheritanceAttribute>(System.Reflection.IntrospectionExtensions.GetTypeInfo(baseType), true))
            {
                if (attribute.Key == discriminatorValue)
                    return attribute.Type;
            }

            return null;
        }

        private string GetSubtypeDiscriminator(System.Type objectType)
        {
            foreach (var attribute in System.Reflection.CustomAttributeExtensions.GetCustomAttributes<JsonInheritanceAttribute>(System.Reflection.IntrospectionExtensions.GetTypeInfo(objectType), true))
            {
                if (attribute.Type == objectType)
                    return attribute.Key;
            }

            return null;
        }
    }



    [System.CodeDom.Compiler.GeneratedCode("NSwag", "14.3.0.0 (NJsonSchema v11.2.0.0 (Newtonsoft.Json v13.0.0.0))")]
    public partial class ApiException : System.Exception
    {
        public int StatusCode { get; private set; }

        public string Response { get; private set; }

        public System.Collections.Generic.IReadOnlyDictionary<string, System.Collections.Generic.IEnumerable<string>> Headers { get; private set; }

        public ApiException(string message, int statusCode, string response, System.Collections.Generic.IReadOnlyDictionary<string, System.Collections.Generic.IEnumerable<string>> headers, System.Exception innerException)
            : base(message + "\n\nStatus: " + statusCode + "\nResponse: \n" + ((response == null) ? "(null)" : response.Substring(0, response.Length >= 512 ? 512 : response.Length)), innerException)
        {
            StatusCode = statusCode;
            Response = response;
            Headers = headers;
        }

        public override string ToString()
        {
            return string.Format("HTTP Response: \n\n{0}\n\n{1}", Response, base.ToString());
        }
    }

    [System.CodeDom.Compiler.GeneratedCode("NSwag", "14.3.0.0 (NJsonSchema v11.2.0.0 (Newtonsoft.Json v13.0.0.0))")]
    public partial class ApiException<TResult> : ApiException
    {
        public TResult Result { get; private set; }

        public ApiException(string message, int statusCode, string response, System.Collections.Generic.IReadOnlyDictionary<string, System.Collections.Generic.IEnumerable<string>> headers, TResult result, System.Exception innerException)
            : base(message, statusCode, response, headers, innerException)
        {
            Result = result;
        }
    }

}

#pragma warning restore  108
#pragma warning restore  114
#pragma warning restore  472
#pragma warning restore  612
#pragma warning restore 1573
#pragma warning restore 1591
#pragma warning restore 8073
#pragma warning restore 3016
#pragma warning restore 8600
#pragma warning restore 8602
#pragma warning restore 8603
#pragma warning restore 8604
#pragma warning restore 8625