using System.Text.Json;
using Bunker.Game.Domain.AggregateModels.Characters;
using Bunker.Game.Domain.AggregateModels.Characters.Cards.CardActions;
using Bunker.Game.Domain.AggregateModels.Characters.Characteristics;
using Microsoft.EntityFrameworkCore;

namespace Bunker.Game.Infrastructure.Data
{
    public class BunkerGameDbContext : DbContext
    {
        public DbSet<Character> Characters { get; set; }

        public BunkerGameDbContext(DbContextOptions<BunkerGameDbContext> options)
            : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Character>().ToTable("characters").HasKey(c => c.Id);

            modelBuilder.Entity<Character>(entity =>
            {
                // Properties
                entity.Property(c => c.GameSessionId).IsRequired();
                entity.HasIndex(c => c.GameSessionId);

                entity.Property(c => c.IsKicked).IsRequired();

                // Owned types (single value objects)
                entity.OwnsOne(
                    c => c.AdditionalInformation,
                    ai =>
                    {
                        ai.Property(a => a.Description).IsRequired().HasMaxLength(500);
                    }
                );

                entity.OwnsOne(
                    c => c.Age,
                    a =>
                    {
                        a.Property(a => a.Years)
                            .IsRequired()
                            .HasComment(
                                $"Must be between {Age.MIN_GAME_CHARACTER_YEARS} and {Age.MAX_GAME_CHARACTER_YEARS}"
                            );
                    }
                );

                entity.OwnsOne(
                    c => c.Childbearing,
                    cb =>
                    {
                        cb.Property(c => c.CanGiveBirth).IsRequired();
                    }
                );

                entity.OwnsOne(
                    c => c.Health,
                    h =>
                    {
                        h.Property(h => h.Description).IsRequired().HasMaxLength(500);
                    }
                );

                entity.OwnsOne(
                    c => c.Hobby,
                    h =>
                    {
                        h.Property(h => h.Description).IsRequired().HasMaxLength(500);
                        h.Property(h => h.Experience)
                            .IsRequired()
                            .HasComment(
                                $"Must be between {Hobby.MIN_GAME_EXPERIENCE_YEARS} and {Hobby.MAX_GAME_EXPERIENCE_YEARS}"
                            );
                    }
                );

                entity.OwnsOne(
                    c => c.Phobia,
                    p =>
                    {
                        p.Property(p => p.Description).IsRequired().HasMaxLength(500);
                    }
                );

                entity.OwnsOne(
                    c => c.Profession,
                    p =>
                    {
                        p.Property(p => p.Description).IsRequired().HasMaxLength(500);
                        p.Property(p => p.ExperienceYears)
                            .IsRequired()
                            .HasComment(
                                $"Must be between {Profession.MIN_GAME_EXPERIENCE_YEARS} and {Profession.MAX_GAME_EXPERIENCE_YEARS}"
                            );
                    }
                );

                entity.OwnsOne(
                    c => c.Sex,
                    s =>
                    {
                        s.Property(s => s.Description).IsRequired().HasMaxLength(50);
                    }
                );

                entity.OwnsOne(
                    c => c.Size,
                    s =>
                    {
                        s.Property(s => s.Height)
                            .IsRequired()
                            .HasComment(
                                $"Must be between {Size.MIN_CHARACTER_HEIGHT} and {Size.MAX_CHARACTER_HEIGHT} cm"
                            );
                        s.Property(s => s.Weight)
                            .IsRequired()
                            .HasComment(
                                $"Must be between {Size.MIN_CHARACTER_WEIGHT} and {Size.MAX_CHARACTER_WEIGHT} kg"
                            );
                    }
                );

                // Owned collections
                entity.OwnsMany(
                    c => c.Items,
                    i =>
                    {
                        i.ToTable("character_items");
                        i.WithOwner().HasForeignKey("character_id");
                        i.Property("Id").HasColumnName("id").ValueGeneratedOnAdd();
                        i.Property(item => item.Description).IsRequired().HasMaxLength(500);
                    }
                );

                entity.OwnsMany(
                    c => c.Traits,
                    t =>
                    {
                        t.ToTable("character_traits");
                        t.WithOwner().HasForeignKey("character_id");
                        t.Property("Id").HasColumnName("id").ValueGeneratedOnAdd();
                        t.Property(trait => trait.Description).IsRequired().HasMaxLength(500);
                    }
                );

                entity.OwnsMany(
                    c => c.Cards,
                    c =>
                    {
                        c.ToTable("character_cards");
                        c.WithOwner().HasForeignKey("character_id");
                        c.Property(card => card.Id).HasColumnName("id");
                        c.Property(card => card.Description).IsRequired().HasMaxLength(500);
                        c.Property(card => card.IsActivated).IsRequired();
                        c.Property(card => card.CardAction)
                            .HasColumnName("card_action")
                            .HasColumnType("jsonb")
                            .IsRequired()
                            .HasConversion(
                                v => JsonSerializer.Serialize(v, (JsonSerializerOptions?)null),
                                v => JsonSerializer.Deserialize<CardAction>(v, (JsonSerializerOptions?)null)!
                            );
                    }
                );
            });
        }
    }
}
