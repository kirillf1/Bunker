using Bunker.GameComponents.API.Entities.CharacterComponents;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bunker.GameComponents.API.Infrastructure.Database.EntityConfigurations;

public class HobbyEntityTypeConfiguration : IEntityTypeConfiguration<HobbyEntity>
{
    public void Configure(EntityTypeBuilder<HobbyEntity> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Description).IsRequired().HasMaxLength(100);
    }
}
