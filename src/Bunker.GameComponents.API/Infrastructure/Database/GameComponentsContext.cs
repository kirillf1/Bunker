using Bunker.GameComponents.API.Entities.BunkerComponents;
using Bunker.GameComponents.API.Entities.CatastropheComponents;
using Bunker.GameComponents.API.Entities.CharacterComponents;
using Bunker.GameComponents.API.Entities.CharacterComponents.Cards;
using Bunker.GameComponents.API.Entities.CharacterComponents.Cards.CardActions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Bunker.GameComponents.API.Infrastructure.Database;

public class GameComponentsContext : DbContext
{
    public const string SCHEMA_NAME = "game_components";
    public DbSet<AdditionalInformationEntity> AdditionalInformationEntitles { get; set; }
    public DbSet<HealthEntity> HealthEntitles { get; set; }
    public DbSet<HobbyEntity> Hobbies { get; set; }
    public DbSet<ItemEntity> Items { get; set; }
    public DbSet<PhobiaEntity> Phobias { get; set; }
    public DbSet<ProfessionEntity> Professions { get; set; }
    public DbSet<TraitEntity> Traits { get; set; }
    public DbSet<CardEntity> Cards { get; set; }
    public DbSet<CardActionEntity> CardActions { get; set; }

    public DbSet<RoomEntity> BunkerRooms { get; set; }
    public DbSet<EnvironmentEntity> BunkerEnvironments { get; set; }
    public DbSet<BunkerItemEntity> BunkerItems { get; set; }

    public DbSet<CatastropheEntity> Catastrophes { get; set; }

    public GameComponentsContext(DbContextOptions<GameComponentsContext> options)
        : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema(SCHEMA_NAME);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(GameComponentsContext).Assembly);

        ConvertAllEnumToString(modelBuilder);
    }

    private static void ConvertAllEnumToString(ModelBuilder modelBuilder)
    {
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            var enumProperties = entityType.GetProperties().Where(p => p.ClrType.IsEnum);
            foreach (var property in enumProperties)
            {
                var enumType = property.ClrType;

                var converterType = typeof(EnumToStringConverter<>).MakeGenericType(enumType);
                var converter = Activator.CreateInstance(converterType) as ValueConverter;

                if (converter != null)
                    property.SetValueConverter(converter);
            }
        }
    }
}
