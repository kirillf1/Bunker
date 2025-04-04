using Bunker.GameComponents.API.Entities.CharacterComponents;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bunker.GameComponents.API.Infrastructure.Database.EntityConfigurations;

public class ProfessionEntityTypeConfiguration : IEntityTypeConfiguration<ProfessionEntity>
{
    public void Configure(EntityTypeBuilder<ProfessionEntity> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Description).IsRequired().HasMaxLength(100);
    }
}
