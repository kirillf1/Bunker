using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bunker.Game.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddedGameSessionCreator : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "is_game_creator",
                table: "game_session_characters",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "is_game_creator",
                table: "game_session_characters");
        }
    }
}
