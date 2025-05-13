using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bunker.Game.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class GameSessionIndexes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "ix_catastrophes_game_session_id",
                table: "catastrophes",
                column: "game_session_id");

            migrationBuilder.CreateIndex(
                name: "ix_bunkers_game_session_id",
                table: "bunkers",
                column: "game_session_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_catastrophes_game_session_id",
                table: "catastrophes");

            migrationBuilder.DropIndex(
                name: "ix_bunkers_game_session_id",
                table: "bunkers");
        }
    }
}
