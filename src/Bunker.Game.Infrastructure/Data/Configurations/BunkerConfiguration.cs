using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BunkerAggregate = Bunker.Game.Domain.AggregateModels.Bunkers;

namespace Bunker.Game.Infrastructure.Data.Configurations;

public class BunkerConfiguration : IEntityTypeConfiguration<BunkerAggregate.BunkerAggregate>
{
    public void Configure(EntityTypeBuilder<BunkerAggregate.BunkerAggregate> builder)
    {
        builder.ToTable("bunkers").HasKey(b => b.Id);

        builder.Property(b => b.GameSessionId).IsRequired();
        builder.Property(b => b.Description).IsRequired().HasMaxLength(1000);
        builder.Property(b => b.IsReadonly).IsRequired();
        builder.HasIndex(b => b.GameSessionId);

        builder.OwnsMany(
            b => b.Rooms,
            r =>
            {
                r.ToTable("bunker_rooms");
                r.WithOwner().HasForeignKey("bunker_id");
                r.Property("Id").HasColumnName("id").ValueGeneratedOnAdd();
                r.Property(r => r.Description).IsRequired().HasMaxLength(500);
                r.Property(r => r.IsHidden).IsRequired();
            }
        );

        builder.OwnsMany(
            b => b.Items,
            i =>
            {
                i.ToTable("bunker_items");
                i.WithOwner().HasForeignKey("bunker_id");
                i.Property("Id").HasColumnName("id").ValueGeneratedOnAdd();
                i.Property(i => i.Description).IsRequired().HasMaxLength(500);
                i.Property(i => i.IsHidden).IsRequired();
            }
        );

        builder.OwnsMany(
            b => b.Environments,
            e =>
            {
                e.ToTable("bunker_environments");
                e.WithOwner().HasForeignKey("bunker_id");
                e.Property("Id").HasColumnName("id").ValueGeneratedOnAdd();
                e.Property(e => e.Description).IsRequired().HasMaxLength(500);
                e.Property(e => e.IsHidden).IsRequired();
            }
        );
    }
}
