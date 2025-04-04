using Bunker.GameComponents.API.Entities.CatastropheComponents;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bunker.GameComponents.API.Infrastructure.Database.EntityConfigurations;

public class CatastropheEntityTypeConfiguration : IEntityTypeConfiguration<CatastropheEntity>
{
    public void Configure(EntityTypeBuilder<CatastropheEntity> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Description).IsRequired().HasMaxLength(1500);
    }
}
