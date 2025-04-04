using Bunker.GameComponents.API.Entities.BunkerComponents;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bunker.GameComponents.API.Infrastructure.Database.EntityConfigurations;

public class EnvironmentEntityTypeConfiguration : IEntityTypeConfiguration<EnvironmentEntity>
{
    public void Configure(EntityTypeBuilder<EnvironmentEntity> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Description).IsRequired().HasMaxLength(300);
    }
}
