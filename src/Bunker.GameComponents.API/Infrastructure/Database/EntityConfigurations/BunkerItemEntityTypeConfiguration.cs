using Bunker.GameComponents.API.Entities.BunkerComponents;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bunker.GameComponents.API.Infrastructure.Database.EntityConfigurations;

public class BunkerItemEntityTypeConfiguration : IEntityTypeConfiguration<BunkerItemEntity>
{
    public void Configure(EntityTypeBuilder<BunkerItemEntity> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Description).IsRequired().HasMaxLength(100);
    }
}
