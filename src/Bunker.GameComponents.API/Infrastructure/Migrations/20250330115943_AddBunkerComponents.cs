using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bunker.GameComponents.API.Migrations;

/// <inheritdoc />
public partial class AddBunkerComponents : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "bunker_environments",
            schema: "game_components",
            columns: table => new
            {
                id = table.Column<Guid>(type: "uuid", nullable: false),
                description = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: false),
            },
            constraints: table =>
            {
                table.PrimaryKey("pk_bunker_environments", x => x.id);
            }
        );

        migrationBuilder.CreateTable(
            name: "bunker_items",
            schema: "game_components",
            columns: table => new
            {
                id = table.Column<Guid>(type: "uuid", nullable: false),
                description = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
            },
            constraints: table =>
            {
                table.PrimaryKey("pk_bunker_items", x => x.id);
            }
        );

        migrationBuilder.CreateTable(
            name: "bunker_rooms",
            schema: "game_components",
            columns: table => new
            {
                id = table.Column<Guid>(type: "uuid", nullable: false),
                description = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
            },
            constraints: table =>
            {
                table.PrimaryKey("pk_bunker_rooms", x => x.id);
            }
        );
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(name: "bunker_environments", schema: "game_components");

        migrationBuilder.DropTable(name: "bunker_items", schema: "game_components");

        migrationBuilder.DropTable(name: "bunker_rooms", schema: "game_components");
    }
}
