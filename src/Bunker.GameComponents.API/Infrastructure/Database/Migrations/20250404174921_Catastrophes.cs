using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bunker.GameComponents.API.Migrations
{
    /// <inheritdoc />
    public partial class Catastrophes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "pk_catastrophe_entity",
                schema: "game_components",
                table: "catastrophe_entity");

            migrationBuilder.RenameTable(
                name: "catastrophe_entity",
                schema: "game_components",
                newName: "catastrophes",
                newSchema: "game_components");

            migrationBuilder.AddPrimaryKey(
                name: "pk_catastrophes",
                schema: "game_components",
                table: "catastrophes",
                column: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "pk_catastrophes",
                schema: "game_components",
                table: "catastrophes");

            migrationBuilder.RenameTable(
                name: "catastrophes",
                schema: "game_components",
                newName: "catastrophe_entity",
                newSchema: "game_components");

            migrationBuilder.AddPrimaryKey(
                name: "pk_catastrophe_entity",
                schema: "game_components",
                table: "catastrophe_entity",
                column: "id");
        }
    }
}
