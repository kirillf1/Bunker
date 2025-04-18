using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bunker.GameComponents.API.Migrations
{
    /// <inheritdoc />
    public partial class AddedBunkerDescription : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "bunker_descriptions",
                schema: "game_components",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    text = table.Column<string>(type: "character varying(1500)", maxLength: 1500, nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_bunker_descriptions", x => x.id);
                }
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "bunker_descriptions", schema: "game_components");
        }
    }
}
