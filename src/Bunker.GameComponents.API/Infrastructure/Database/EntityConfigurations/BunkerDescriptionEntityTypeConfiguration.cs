using Bunker.GameComponents.API.Entities.BunkerComponents;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bunker.GameComponents.API.Infrastructure.Database.EntityConfigurations
{
    public class BunkerDescriptionEntityTypeConfiguration : IEntityTypeConfiguration<BunkerDescriptionEntity>
    {
        public void Configure(EntityTypeBuilder<BunkerDescriptionEntity> builder)
        {
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Text).IsRequired().HasMaxLength(1500);
        }
    }
}
