using System.Text.Json;
using System.Text.Json.Serialization;
using Bunker.Game.Domain.AggregateModels.Catastrophes;
using Bunker.Game.Domain.AggregateModels.GameSessions;
using Bunker.Game.Infrastructure.Data.Configurations;
using Microsoft.EntityFrameworkCore;
using BunkerAggregate = Bunker.Game.Domain.AggregateModels.Bunkers;

namespace Bunker.Game.Infrastructure.Data
{
    public class BunkerGameDbContext : DbContext
    {
        public DbSet<GameSession> GameSessions { get; set; }
        public DbSet<BunkerAggregate.Bunker> Bunkers { get; set; }
        public DbSet<Catastrophe> Catastrophes { get; set; }

        public BunkerGameDbContext(DbContextOptions<BunkerGameDbContext> options)
            : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(BunkerGameDbContext).Assembly);
        }
    }
}
