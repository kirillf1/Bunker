using Bunker.GameComponents.API.Entities.CharacterComponents;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bunker.GameComponents.API.Infrastructure.EntityConfigurations
{
    public class TraitEntityTypeConfiguration : IEntityTypeConfiguration<TraitEntity>
    {
        public void Configure(EntityTypeBuilder<TraitEntity> builder)
        {
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Description).IsRequired().HasMaxLength(100);
        }
    }
}
