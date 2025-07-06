using Bunker.Game.Domain.AggregateModels.GameSessions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bunker.Game.Infrastructure.Data.Configurations;

public class GameSessionConfiguration : IEntityTypeConfiguration<GameSession>
{
    public void Configure(EntityTypeBuilder<GameSession> builder)
    {
        builder.ToTable("game_sessions").HasKey(g => g.Id);

        builder.Property(g => g.Name).IsRequired().HasMaxLength(100);
        builder.Property(g => g.GameState).IsRequired();
        builder.Property(g => g.GameState).HasConversion(v => v.ToString(), v => Enum.Parse<GameState>(v));
        builder.Property(g => g.FreeSeatsCount).IsRequired();
        builder.Property(g => g.GameResultDescription).HasMaxLength(20000);

        builder.OwnsMany(
            g => g.Characters,
            c =>
            {
                c.ToTable("game_session_characters");
                c.WithOwner().HasForeignKey("game_session_id");
                c.Property("Id").HasColumnName("id").ValueGeneratedOnAdd();
                c.Property(c => c.IsKicked).IsRequired();

                c.OwnsOne(
                    c => c.Player,
                    p =>
                    {
                        p.Property(p => p.Id).IsRequired();
                        p.HasIndex(p => p.Id);
                        p.Property(p => p.Name).IsRequired().HasMaxLength(100);
                    }
                );
            }
        );
    }
}
