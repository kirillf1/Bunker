using Bunker.GameComponents.API.Entities.CharacterComponents.Cards;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bunker.GameComponents.API.Infrastructure.EntityConfigurations;

public class CardEntityTypeConfiguration : IEntityTypeConfiguration<CardEntity>
{
    public void Configure(EntityTypeBuilder<CardEntity> builder)
    {
        builder.Property(c => c.Description).IsRequired().HasMaxLength(1000);
        builder.HasOne(c => c.CardAction).WithOne(c => c.CardEntity).OnDelete(DeleteBehavior.Cascade);
        builder.Navigation(c => c.CardAction).AutoInclude();
    }
}
