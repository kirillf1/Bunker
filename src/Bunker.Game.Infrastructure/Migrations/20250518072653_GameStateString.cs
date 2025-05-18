using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bunker.Game.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class GameStateString : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "game_state",
                table: "game_sessions",
                type: "text",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "game_state",
                table: "game_sessions",
                type: "integer",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");
        }
    }
}
