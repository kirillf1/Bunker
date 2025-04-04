using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bunker.GameComponents.API.Migrations;

/// <inheritdoc />
public partial class Intial : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.EnsureSchema(name: "game_components");

        migrationBuilder.CreateTable(
            name: "additional_information_entitles",
            schema: "game_components",
            columns: table => new
            {
                id = table.Column<Guid>(type: "uuid", nullable: false),
                description = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
            },
            constraints: table =>
            {
                table.PrimaryKey("pk_additional_information_entitles", x => x.id);
            }
        );

        migrationBuilder.CreateTable(
            name: "card_actions",
            schema: "game_components",
            columns: table => new
            {
                id = table.Column<Guid>(type: "uuid", nullable: false),
                discriminator = table.Column<string>(type: "character varying(34)", maxLength: 34, nullable: false),
                characteristic_type = table.Column<string>(type: "text", nullable: true),
                characteristic_id = table.Column<Guid>(type: "uuid", nullable: true),
                target_characters_count = table.Column<int>(type: "integer", nullable: true),
                exchange_characteristic_action_entity_characteristic_type = table.Column<string>(
                    type: "text",
                    nullable: true
                ),
                recreate_character_action_entity_target_characters_count = table.Column<int>(
                    type: "integer",
                    nullable: true
                ),
                remove_characteristic_card_action_entity_characteristic_type = table.Column<string>(
                    type: "text",
                    nullable: true
                ),
                remove_characteristic_card_action_entity_target_characters_count = table.Column<int>(
                    type: "integer",
                    nullable: true
                ),
                reroll_characteristic_card_action_entity_characteristic_type = table.Column<string>(
                    type: "text",
                    nullable: true
                ),
                is_self_target = table.Column<bool>(type: "boolean", nullable: true),
                reroll_characteristic_card_action_entity_characteristic_id = table.Column<Guid>(
                    type: "uuid",
                    nullable: true
                ),
                reroll_characteristic_card_action_entity_target_characters_count = table.Column<int>(
                    type: "integer",
                    nullable: true
                ),
                bunker_object_type = table.Column<string>(type: "text", nullable: true),
                spy_characteristic_card_action_entity_characteristic_type = table.Column<string>(
                    type: "text",
                    nullable: true
                ),
                spy_characteristic_card_action_entity_target_characters_count = table.Column<int>(
                    type: "integer",
                    nullable: true
                ),
            },
            constraints: table =>
            {
                table.PrimaryKey("pk_card_actions", x => x.id);
            }
        );

        migrationBuilder.CreateTable(
            name: "health_entitles",
            schema: "game_components",
            columns: table => new
            {
                id = table.Column<Guid>(type: "uuid", nullable: false),
                description = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
            },
            constraints: table =>
            {
                table.PrimaryKey("pk_health_entitles", x => x.id);
            }
        );

        migrationBuilder.CreateTable(
            name: "hobbies",
            schema: "game_components",
            columns: table => new
            {
                id = table.Column<Guid>(type: "uuid", nullable: false),
                description = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
            },
            constraints: table =>
            {
                table.PrimaryKey("pk_hobbies", x => x.id);
            }
        );

        migrationBuilder.CreateTable(
            name: "items",
            schema: "game_components",
            columns: table => new
            {
                id = table.Column<Guid>(type: "uuid", nullable: false),
                description = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
            },
            constraints: table =>
            {
                table.PrimaryKey("pk_items", x => x.id);
            }
        );

        migrationBuilder.CreateTable(
            name: "phobias",
            schema: "game_components",
            columns: table => new
            {
                id = table.Column<Guid>(type: "uuid", nullable: false),
                description = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: false),
            },
            constraints: table =>
            {
                table.PrimaryKey("pk_phobias", x => x.id);
            }
        );

        migrationBuilder.CreateTable(
            name: "professions",
            schema: "game_components",
            columns: table => new
            {
                id = table.Column<Guid>(type: "uuid", nullable: false),
                description = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
            },
            constraints: table =>
            {
                table.PrimaryKey("pk_professions", x => x.id);
            }
        );

        migrationBuilder.CreateTable(
            name: "traits",
            schema: "game_components",
            columns: table => new
            {
                id = table.Column<Guid>(type: "uuid", nullable: false),
                description = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
            },
            constraints: table =>
            {
                table.PrimaryKey("pk_traits", x => x.id);
            }
        );

        migrationBuilder.CreateTable(
            name: "cards",
            schema: "game_components",
            columns: table => new
            {
                id = table.Column<Guid>(type: "uuid", nullable: false),
                description = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                card_action_id = table.Column<Guid>(type: "uuid", nullable: false),
            },
            constraints: table =>
            {
                table.PrimaryKey("pk_cards", x => x.id);
                table.ForeignKey(
                    name: "fk_cards_card_actions_card_action_id",
                    column: x => x.card_action_id,
                    principalSchema: "game_components",
                    principalTable: "card_actions",
                    principalColumn: "id",
                    onDelete: ReferentialAction.Cascade
                );
            }
        );

        migrationBuilder.CreateIndex(
            name: "ix_cards_card_action_id",
            schema: "game_components",
            table: "cards",
            column: "card_action_id",
            unique: true
        );
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(name: "additional_information_entitles", schema: "game_components");

        migrationBuilder.DropTable(name: "cards", schema: "game_components");

        migrationBuilder.DropTable(name: "health_entitles", schema: "game_components");

        migrationBuilder.DropTable(name: "hobbies", schema: "game_components");

        migrationBuilder.DropTable(name: "items", schema: "game_components");

        migrationBuilder.DropTable(name: "phobias", schema: "game_components");

        migrationBuilder.DropTable(name: "professions", schema: "game_components");

        migrationBuilder.DropTable(name: "traits", schema: "game_components");

        migrationBuilder.DropTable(name: "card_actions", schema: "game_components");
    }
}
