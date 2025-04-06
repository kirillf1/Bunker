using Bunker.GameComponents.API.Entities.CharacterComponents.Cards.CardActions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bunker.GameComponents.API.Infrastructure.Database.EntityConfigurations;

public class CardActionEntityTypeConfiguration : IEntityTypeConfiguration<CardActionEntity>
{
    public const string DiscriminatorName = "card_action_type";

    public static readonly IReadOnlyDictionary<Type, string> DiscriminatorValues = new Dictionary<Type, string>
    {
        { typeof(AddCharacteristicEntity), "AddCharacteristic" },
        { typeof(EmptyActionEntity), "EmptyAction" },
        { typeof(ExchangeCharacteristicActionEntity), "ExchangeCharacteristic" },
        { typeof(RecreateCharacterActionEntity), "RecreateCharacter" },
        { typeof(RemoveCharacteristicCardActionEntity), "RemoveCharacteristic" },
        { typeof(RerollCharacteristicCardActionEntity), "RerollCharacteristic" },
        { typeof(RevealBunkerGameComponentCardActionEntity), "RevealBunkerGameComponent" },
        { typeof(SpyCharacteristicCardActionEntity), "SpyCharacteristic" },
        { typeof(RecreateBunkerActionEntity), "RecreateBunker" },
        { typeof(RecreateCatastropheActionEntity), "RecreateCatastrophe" },
    };

    public void Configure(EntityTypeBuilder<CardActionEntity> builder)
    {
        builder.HasKey(c => c.Id);

        builder
            .HasDiscriminator<string>("card_action_type")
            .HasValue<AddCharacteristicEntity>(DiscriminatorValues[typeof(AddCharacteristicEntity)])
            .HasValue<EmptyActionEntity>(DiscriminatorValues[typeof(EmptyActionEntity)])
            .HasValue<ExchangeCharacteristicActionEntity>(
                DiscriminatorValues[typeof(ExchangeCharacteristicActionEntity)]
            )
            .HasValue<RecreateCharacterActionEntity>(DiscriminatorValues[typeof(RecreateCharacterActionEntity)])
            .HasValue<RemoveCharacteristicCardActionEntity>(
                DiscriminatorValues[typeof(RemoveCharacteristicCardActionEntity)]
            )
            .HasValue<RerollCharacteristicCardActionEntity>(
                DiscriminatorValues[typeof(RerollCharacteristicCardActionEntity)]
            )
            .HasValue<RevealBunkerGameComponentCardActionEntity>(
                DiscriminatorValues[typeof(RevealBunkerGameComponentCardActionEntity)]
            )
            .HasValue<SpyCharacteristicCardActionEntity>(DiscriminatorValues[typeof(SpyCharacteristicCardActionEntity)])
            .HasValue<RecreateBunkerActionEntity>(DiscriminatorValues[typeof(RecreateBunkerActionEntity)])
            .HasValue<RecreateCatastropheActionEntity>(DiscriminatorValues[typeof(RecreateCatastropheActionEntity)]);
    }
}
