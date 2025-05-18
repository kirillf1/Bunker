using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bunker.Game.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CharacterComponentsIndexes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "ix_character_traits_character_id",
                table: "character_traits",
                column: "character_id");

            migrationBuilder.CreateIndex(
                name: "ix_character_items_character_id",
                table: "character_items",
                column: "character_id");

            migrationBuilder.CreateIndex(
                name: "ix_character_cards_character_id",
                table: "character_cards",
                column: "character_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_character_traits_character_id",
                table: "character_traits");

            migrationBuilder.DropIndex(
                name: "ix_character_items_character_id",
                table: "character_items");

            migrationBuilder.DropIndex(
                name: "ix_character_cards_character_id",
                table: "character_cards");
        }
    }
}
