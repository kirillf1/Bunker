using Bunker.GameComponents.API.Entities.CharacterComponents.Cards.CardActions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bunker.GameComponents.API.Infrastructure.Database.EntityConfigurations;

public class CardActionEntityTypeConfiguration : IEntityTypeConfiguration<CardActionEntity>
{
    public void Configure(EntityTypeBuilder<CardActionEntity> builder)
    {
        builder.HasKey(c => c.Id);

        builder
            .HasDiscriminator()
            .HasValue<AddCharacteristicEntity>("AddCharacteristic")
            .HasValue<EmptyActionEntity>("EmptyAction")
            .HasValue<ExchangeCharacteristicActionEntity>("ExchangeCharacteristic")
            .HasValue<RecreateCharacterActionEntity>("RecreateCharacter")
            .HasValue<RemoveCharacteristicCardActionEntity>("RemoveCharacteristic")
            .HasValue<RerollCharacteristicCardActionEntity>("RerollCharacteristic")
            .HasValue<RevealBunkerGameComponentCardActionEntity>("RevealBunkerGameComponent")
            .HasValue<SpyCharacteristicCardActionEntity>("SpyCharacteristic")
            .HasValue<RecreateBunkerActionEntity>("RecreateBunker")
            .HasValue<RecreateCatastropheActionEntity>("RecreateCatastrophe");
    }
}
