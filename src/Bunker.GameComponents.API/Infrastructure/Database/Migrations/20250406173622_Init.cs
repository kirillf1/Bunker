using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bunker.GameComponents.API.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "game_components");

            migrationBuilder.CreateTable(
                name: "additional_information_entitles",
                schema: "game_components",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    description = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_additional_information_entitles", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "bunker_environments",
                schema: "game_components",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    description = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_bunker_environments", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "bunker_items",
                schema: "game_components",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    description = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_bunker_items", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "bunker_rooms",
                schema: "game_components",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    description = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_bunker_rooms", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "cards",
                schema: "game_components",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    description = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    card_action = table.Column<string>(type: "jsonb", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_cards", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "catastrophes",
                schema: "game_components",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    description = table.Column<string>(type: "character varying(1500)", maxLength: 1500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_catastrophes", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "health_entitles",
                schema: "game_components",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    description = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_health_entitles", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "hobbies",
                schema: "game_components",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    description = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_hobbies", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "items",
                schema: "game_components",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    description = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_items", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "phobias",
                schema: "game_components",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    description = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_phobias", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "professions",
                schema: "game_components",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    description = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_professions", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "traits",
                schema: "game_components",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    description = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_traits", x => x.id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "additional_information_entitles",
                schema: "game_components");

            migrationBuilder.DropTable(
                name: "bunker_environments",
                schema: "game_components");

            migrationBuilder.DropTable(
                name: "bunker_items",
                schema: "game_components");

            migrationBuilder.DropTable(
                name: "bunker_rooms",
                schema: "game_components");

            migrationBuilder.DropTable(
                name: "cards",
                schema: "game_components");

            migrationBuilder.DropTable(
                name: "catastrophes",
                schema: "game_components");

            migrationBuilder.DropTable(
                name: "health_entitles",
                schema: "game_components");

            migrationBuilder.DropTable(
                name: "hobbies",
                schema: "game_components");

            migrationBuilder.DropTable(
                name: "items",
                schema: "game_components");

            migrationBuilder.DropTable(
                name: "phobias",
                schema: "game_components");

            migrationBuilder.DropTable(
                name: "professions",
                schema: "game_components");

            migrationBuilder.DropTable(
                name: "traits",
                schema: "game_components");
        }
    }
}
