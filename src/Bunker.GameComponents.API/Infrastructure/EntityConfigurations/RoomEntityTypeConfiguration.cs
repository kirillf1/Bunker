using Bunker.GameComponents.API.Entities.BunkerComponents;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bunker.GameComponents.API.Infrastructure.EntityConfigurations
{
    public class RoomEntityTypeConfiguration : IEntityTypeConfiguration<RoomEntity>
    {
        public void Configure(EntityTypeBuilder<RoomEntity> builder)
        {
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Description).IsRequired().HasMaxLength(100);
        }
    }
}
