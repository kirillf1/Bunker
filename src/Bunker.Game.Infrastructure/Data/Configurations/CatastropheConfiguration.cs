using Bunker.Game.Domain.AggregateModels.Catastrophes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bunker.Game.Infrastructure.Data.Configurations;

public class CatastropheConfiguration : IEntityTypeConfiguration<Catastrophe>
{
    public void Configure(EntityTypeBuilder<Catastrophe> builder)
    {
        builder.ToTable("catastrophes").HasKey(c => c.Id);

        builder.Property(c => c.GameSessionId).IsRequired();
        builder.Property(c => c.Description).IsRequired().HasMaxLength(1000);
        builder.Property(c => c.IsReadOnly).IsRequired();
    }
}
