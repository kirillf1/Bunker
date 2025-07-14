using System.Text.Json;
using System.Text.Json.Serialization;
using Bunker.Game.Domain.AggregateModels.Characters;
using Bunker.Game.Domain.AggregateModels.Characters.Cards.CardActions;
using Bunker.Game.Domain.AggregateModels.Characters.Characteristics;
using Bunker.Game.Infrastructure.Data.Converters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bunker.Game.Infrastructure.Data.Configurations;

public class CharacterConfiguration : IEntityTypeConfiguration<Character>
{
    public void Configure(EntityTypeBuilder<Character> builder)
    {
        builder.ToTable("characters").HasKey(c => c.Id);

        // Properties
        builder.Property(c => c.GameSessionId).IsRequired();
        builder.HasIndex(c => c.GameSessionId);

        builder.Property(c => c.IsKicked).IsRequired();

        // Owned types (single value objects)
        builder.OwnsOne(
            c => c.AdditionalInformation,
            ai =>
            {
                ai.Property(a => a.Description).IsRequired().HasMaxLength(500);
            }
        );

        builder.OwnsOne(
            c => c.Age,
            a =>
            {
                a.Property(a => a.Years)
                    .IsRequired()
                    .HasComment($"Must be between {Age.MIN_GAME_CHARACTER_YEARS} and {Age.MAX_GAME_CHARACTER_YEARS}");
            }
        );

        builder.OwnsOne(
            c => c.Childbearing,
            cb =>
            {
                cb.Property(c => c.CanGiveBirth).IsRequired();
            }
        );

        builder.OwnsOne(
            c => c.Health,
            h =>
            {
                h.Property(h => h.Description).IsRequired().HasMaxLength(500);
            }
        );

        builder.OwnsOne(
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

        builder.OwnsOne(
            c => c.Phobia,
            p =>
            {
                p.Property(p => p.Description).IsRequired().HasMaxLength(500);
            }
        );

        builder.OwnsOne(
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

        builder.OwnsOne(
            c => c.Sex,
            s =>
            {
                s.Property(s => s.Description).IsRequired().HasMaxLength(50);
            }
        );

        builder.OwnsOne(
            c => c.Size,
            s =>
            {
                s.Property(s => s.Height)
                    .IsRequired()
                    .HasComment($"Must be between {Size.MIN_CHARACTER_HEIGHT} and {Size.MAX_CHARACTER_HEIGHT} cm");
                s.Property(s => s.Weight)
                    .IsRequired()
                    .HasComment($"Must be between {Size.MIN_CHARACTER_WEIGHT} and {Size.MAX_CHARACTER_WEIGHT} kg");
            }
        );

        // Owned collections
        builder.OwnsMany(
            c => c.Items,
            i =>
            {
                i.ToTable("character_items");
                i.WithOwner().HasForeignKey("character_id");
                i.HasIndex("character_id");
                i.Property("Id").HasColumnName("id").ValueGeneratedOnAdd();
                i.Property(item => item.Description).IsRequired().HasMaxLength(500);
            }
        );

        builder.OwnsMany(
            c => c.Traits,
            t =>
            {
                t.ToTable("character_traits");
                t.WithOwner().HasForeignKey("character_id");
                t.HasIndex("character_id");
                t.Property("Id").HasColumnName("id").ValueGeneratedOnAdd();
                t.Property(trait => trait.Description).IsRequired().HasMaxLength(500);
            }
        );

        var jsonSerializerOptions = new JsonSerializerOptions();
        jsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        jsonSerializerOptions.Converters.Add(new CardActionJsonConverter());

        builder.OwnsMany(
            c => c.Cards,
            c =>
            {
                c.ToTable("character_cards");
                c.WithOwner().HasForeignKey("character_id");
                c.HasIndex("character_id");
                c.Property(card => card.Id).HasColumnName("id");
                c.Property(card => card.Description).IsRequired().HasMaxLength(500);
                c.Property(card => card.IsActivated).IsRequired();
                c.Property(card => card.CardAction)
                    .HasColumnName("card_action")
                    .HasColumnType("jsonb")
                    .IsRequired()
                    .HasConversion(
                        c => JsonSerializer.Serialize(c, jsonSerializerOptions),
                        x => JsonSerializer.Deserialize<CardAction>(x, jsonSerializerOptions)!
                    );
            }
        );
    }
}
