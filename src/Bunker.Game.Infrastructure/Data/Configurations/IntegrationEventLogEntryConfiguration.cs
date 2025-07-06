using Bunker.MessageBus.Abstractions.IntegrationEventLogs;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bunker.Game.Infrastructure.Data.Configurations;

public class IntegrationEventLogEntryConfiguration : IEntityTypeConfiguration<IntegrationEventLogEntry>
{
    public void Configure(EntityTypeBuilder<IntegrationEventLogEntry> builder)
    {
        builder.ToTable("integration_event_log").HasKey(e => e.EventId);

        builder.Property(e => e.EventId).IsRequired();
        builder.Property(e => e.EventTypeName).IsRequired().HasMaxLength(500);
        builder.Property(e => e.Content).IsRequired();
        builder.Property(e => e.State).IsRequired();
        builder.Property(e => e.TimesSent).IsRequired();
        builder.Property(e => e.CreationTime).IsRequired();
        builder.Property(e => e.TransactionId).IsRequired();

        builder.HasIndex(e => new { e.State, e.CreationTime });
        builder.HasIndex(e => e.TransactionId);
    }
} 