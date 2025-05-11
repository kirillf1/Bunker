using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Bunker.Game.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class GameSessionWithBunker : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "character_cards");

            migrationBuilder.DropTable(
                name: "character_items");

            migrationBuilder.DropTable(
                name: "character_traits");

            migrationBuilder.DropTable(
                name: "characters");

            migrationBuilder.CreateTable(
                name: "bunkers",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    game_session_id = table.Column<Guid>(type: "uuid", nullable: false),
                    description = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    is_readonly = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_bunkers", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "catastrophes",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    game_session_id = table.Column<Guid>(type: "uuid", nullable: false),
                    description = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    is_read_only = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_catastrophes", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "game_sessions",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    game_state = table.Column<int>(type: "integer", nullable: false),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    free_seats_count = table.Column<int>(type: "integer", nullable: false),
                    game_result_description = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_game_sessions", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "bunker_environments",
                columns: table => new
                {
                    bunker_id = table.Column<Guid>(type: "uuid", nullable: false),
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    is_hidden = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_bunker_environments", x => new { x.bunker_id, x.id });
                    table.ForeignKey(
                        name: "fk_bunker_environments_bunkers_bunker_id",
                        column: x => x.bunker_id,
                        principalTable: "bunkers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "bunker_items",
                columns: table => new
                {
                    bunker_id = table.Column<Guid>(type: "uuid", nullable: false),
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    is_hidden = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_bunker_items", x => new { x.bunker_id, x.id });
                    table.ForeignKey(
                        name: "fk_bunker_items_bunkers_bunker_id",
                        column: x => x.bunker_id,
                        principalTable: "bunkers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "bunker_rooms",
                columns: table => new
                {
                    bunker_id = table.Column<Guid>(type: "uuid", nullable: false),
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    is_hidden = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_bunker_rooms", x => new { x.bunker_id, x.id });
                    table.ForeignKey(
                        name: "fk_bunker_rooms_bunkers_bunker_id",
                        column: x => x.bunker_id,
                        principalTable: "bunkers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "game_session_characters",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    game_session_id = table.Column<Guid>(type: "uuid", nullable: false),
                    player_name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    player_id = table.Column<string>(type: "text", nullable: true),
                    is_kicked = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_game_session_characters", x => new { x.game_session_id, x.id });
                    table.ForeignKey(
                        name: "fk_game_session_characters_game_sessions_game_session_id",
                        column: x => x.game_session_id,
                        principalTable: "game_sessions",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "bunker_environments");

            migrationBuilder.DropTable(
                name: "bunker_items");

            migrationBuilder.DropTable(
                name: "bunker_rooms");

            migrationBuilder.DropTable(
                name: "catastrophes");

            migrationBuilder.DropTable(
                name: "game_session_characters");

            migrationBuilder.DropTable(
                name: "bunkers");

            migrationBuilder.DropTable(
                name: "game_sessions");

            migrationBuilder.CreateTable(
                name: "characters",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    game_session_id = table.Column<Guid>(type: "uuid", nullable: false),
                    is_kicked = table.Column<bool>(type: "boolean", nullable: false),
                    additional_information_description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    age_years = table.Column<int>(type: "integer", nullable: false, comment: "Must be between 17 and 100"),
                    childbearing_can_give_birth = table.Column<bool>(type: "boolean", nullable: false),
                    health_description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    hobby_description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    hobby_experience = table.Column<byte>(type: "smallint", nullable: false, comment: "Must be between 1 and 3"),
                    phobia_description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    profession_description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    profession_experience_years = table.Column<byte>(type: "smallint", nullable: false, comment: "Must be between 1 and 5"),
                    sex_description = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    size_height = table.Column<double>(type: "double precision", nullable: false, comment: "Must be between 130 and 210 cm"),
                    size_weight = table.Column<double>(type: "double precision", nullable: false, comment: "Must be between 40 and 150 kg")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_characters", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "character_cards",
                columns: table => new
                {
                    character_id = table.Column<Guid>(type: "uuid", nullable: false),
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    card_action = table.Column<string>(type: "text", nullable: false),
                    description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    is_activated = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_character_cards", x => new { x.character_id, x.id });
                    table.ForeignKey(
                        name: "fk_character_cards_characters_character_id",
                        column: x => x.character_id,
                        principalTable: "characters",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "character_items",
                columns: table => new
                {
                    character_id = table.Column<Guid>(type: "uuid", nullable: false),
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_character_items", x => new { x.character_id, x.id });
                    table.ForeignKey(
                        name: "fk_character_items_characters_character_id",
                        column: x => x.character_id,
                        principalTable: "characters",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "character_traits",
                columns: table => new
                {
                    character_id = table.Column<Guid>(type: "uuid", nullable: false),
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_character_traits", x => new { x.character_id, x.id });
                    table.ForeignKey(
                        name: "fk_character_traits_characters_character_id",
                        column: x => x.character_id,
                        principalTable: "characters",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_characters_game_session_id",
                table: "characters",
                column: "game_session_id");
        }
    }
}
