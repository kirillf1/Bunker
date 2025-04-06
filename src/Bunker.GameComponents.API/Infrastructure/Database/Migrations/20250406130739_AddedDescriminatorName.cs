using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bunker.GameComponents.API.Migrations
{
    /// <inheritdoc />
    public partial class AddedDescriminatorName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "discriminator",
                schema: "game_components",
                table: "card_actions",
                newName: "card_action_type");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "card_action_type",
                schema: "game_components",
                table: "card_actions",
                newName: "discriminator");
        }
    }
}
