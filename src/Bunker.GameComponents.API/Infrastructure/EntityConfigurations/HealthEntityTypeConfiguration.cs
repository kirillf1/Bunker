using Bunker.GameComponents.API.Entities.CharacterComponents;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bunker.GameComponents.API.Infrastructure.EntityConfigurations;

public class HealthEntityTypeConfiguration : IEntityTypeConfiguration<HealthEntity>
{
    public void Configure(EntityTypeBuilder<HealthEntity> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Description).IsRequired().HasMaxLength(100);
    }
}
