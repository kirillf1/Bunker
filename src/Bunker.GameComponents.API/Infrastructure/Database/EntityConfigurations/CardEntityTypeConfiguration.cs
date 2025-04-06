using System.Text.Json;
using System.Text.Json.Serialization;
using Bunker.GameComponents.API.Entities.CharacterComponents.Cards;
using Bunker.GameComponents.API.Entities.CharacterComponents.Cards.CardActions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bunker.GameComponents.API.Infrastructure.Database.EntityConfigurations;

public class CardEntityTypeConfiguration : IEntityTypeConfiguration<CardEntity>
{
    public void Configure(EntityTypeBuilder<CardEntity> builder)
    {
        builder.Property(c => c.Description).IsRequired().HasMaxLength(1000);

        var jsonSerializerOptions = new JsonSerializerOptions();
        jsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());

        builder
            .Property(c => c.CardAction)
            .HasColumnName("card_action")
            .HasColumnType("jsonb")
            .HasConversion(
                c => JsonSerializer.Serialize(c, jsonSerializerOptions),
                x => JsonSerializer.Deserialize<CardActionEntity>(x, jsonSerializerOptions)!
            );
    }
}
