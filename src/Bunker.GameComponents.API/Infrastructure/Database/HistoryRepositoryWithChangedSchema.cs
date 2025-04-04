using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Migrations.Internal;

namespace Bunker.GameComponents.API.Infrastructure.Database;

[System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "EF1001:Internal EF Core API usage.")]
public class HistoryRepositoryWithChangedSchema : NpgsqlHistoryRepository
{
    public HistoryRepositoryWithChangedSchema(HistoryRepositoryDependencies dependencies)
        : base(dependencies) { }

    protected override string? TableSchema => GameComponentsContext.SCHEMA_NAME;
}
