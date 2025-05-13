using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Bunker.Game.Infrastructure.Migrations;

/// <inheritdoc />
public partial class Characters : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "characters",
            columns: table => new
            {
                id = table.Column<Guid>(type: "uuid", nullable: false),
                game_session_id = table.Column<Guid>(type: "uuid", nullable: false),
                additional_information_description = table.Column<string>(
                    type: "character varying(500)",
                    maxLength: 500,
                    nullable: false
                ),
                age_years = table.Column<int>(type: "integer", nullable: false, comment: "Must be between 17 and 100"),
                childbearing_can_give_birth = table.Column<bool>(type: "boolean", nullable: false),
                health_description = table.Column<string>(
                    type: "character varying(500)",
                    maxLength: 500,
                    nullable: false
                ),
                hobby_description = table.Column<string>(
                    type: "character varying(500)",
                    maxLength: 500,
                    nullable: false
                ),
                hobby_experience = table.Column<byte>(
                    type: "smallint",
                    nullable: false,
                    comment: "Must be between 1 and 3"
                ),
                phobia_description = table.Column<string>(
                    type: "character varying(500)",
                    maxLength: 500,
                    nullable: false
                ),
                profession_description = table.Column<string>(
                    type: "character varying(500)",
                    maxLength: 500,
                    nullable: false
                ),
                profession_experience_years = table.Column<byte>(
                    type: "smallint",
                    nullable: false,
                    comment: "Must be between 1 and 5"
                ),
                sex_description = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                size_height = table.Column<double>(
                    type: "double precision",
                    nullable: false,
                    comment: "Must be between 130 and 210 cm"
                ),
                size_weight = table.Column<double>(
                    type: "double precision",
                    nullable: false,
                    comment: "Must be between 40 and 150 kg"
                ),
                is_kicked = table.Column<bool>(type: "boolean", nullable: false),
            },
            constraints: table =>
            {
                table.PrimaryKey("pk_characters", x => x.id);
            }
        );

        migrationBuilder.CreateTable(
            name: "character_cards",
            columns: table => new
            {
                id = table.Column<Guid>(type: "uuid", nullable: false),
                character_id = table.Column<Guid>(type: "uuid", nullable: false),
                is_activated = table.Column<bool>(type: "boolean", nullable: false),
                description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                card_action = table.Column<string>(type: "jsonb", nullable: false),
            },
            constraints: table =>
            {
                table.PrimaryKey("pk_character_cards", x => new { x.character_id, x.id });
                table.ForeignKey(
                    name: "fk_character_cards_characters_character_id",
                    column: x => x.character_id,
                    principalTable: "characters",
                    principalColumn: "id",
                    onDelete: ReferentialAction.Cascade
                );
            }
        );

        migrationBuilder.CreateTable(
            name: "character_items",
            columns: table => new
            {
                character_id = table.Column<Guid>(type: "uuid", nullable: false),
                id = table
                    .Column<int>(type: "integer", nullable: false)
                    .Annotation(
                        "Npgsql:ValueGenerationStrategy",
                        NpgsqlValueGenerationStrategy.IdentityByDefaultColumn
                    ),
                description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
            },
            constraints: table =>
            {
                table.PrimaryKey("pk_character_items", x => new { x.character_id, x.id });
                table.ForeignKey(
                    name: "fk_character_items_characters_character_id",
                    column: x => x.character_id,
                    principalTable: "characters",
                    principalColumn: "id",
                    onDelete: ReferentialAction.Cascade
                );
            }
        );

        migrationBuilder.CreateTable(
            name: "character_traits",
            columns: table => new
            {
                character_id = table.Column<Guid>(type: "uuid", nullable: false),
                id = table
                    .Column<int>(type: "integer", nullable: false)
                    .Annotation(
                        "Npgsql:ValueGenerationStrategy",
                        NpgsqlValueGenerationStrategy.IdentityByDefaultColumn
                    ),
                description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
            },
            constraints: table =>
            {
                table.PrimaryKey("pk_character_traits", x => new { x.character_id, x.id });
                table.ForeignKey(
                    name: "fk_character_traits_characters_character_id",
                    column: x => x.character_id,
                    principalTable: "characters",
                    principalColumn: "id",
                    onDelete: ReferentialAction.Cascade
                );
            }
        );

        migrationBuilder.CreateIndex(
            name: "ix_game_session_characters_player_id",
            table: "game_session_characters",
            column: "player_id"
        );

        migrationBuilder.CreateIndex(
            name: "ix_characters_game_session_id",
            table: "characters",
            column: "game_session_id"
        );
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(name: "character_cards");

        migrationBuilder.DropTable(name: "character_items");

        migrationBuilder.DropTable(name: "character_traits");

        migrationBuilder.DropTable(name: "characters");

        migrationBuilder.DropIndex(name: "ix_game_session_characters_player_id", table: "game_session_characters");
    }
}
