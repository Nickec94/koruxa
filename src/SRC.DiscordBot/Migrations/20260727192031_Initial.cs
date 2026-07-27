using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SRC.DiscordBot.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "koruxa_boss",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    created_at = table.Column<DateTimeOffset>(type: "TEXT", nullable: false),
                    killed_at = table.Column<DateTimeOffset>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_koruxa_boss", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "koruxa_user",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    discord_user_id = table.Column<ulong>(type: "INTEGER", nullable: false),
                    last_alert_send_at = table.Column<DateTimeOffset>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_koruxa_user", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "koruxa_boss_attack",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    boss_id = table.Column<int>(type: "INTEGER", nullable: false),
                    discord_user_id = table.Column<ulong>(type: "INTEGER", nullable: false),
                    attacked_at = table.Column<DateTimeOffset>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_koruxa_boss_attack", x => x.id);
                    table.ForeignKey(
                        name: "fk_koruxa_boss_attack_koruxa_boss_boss_id",
                        column: x => x.boss_id,
                        principalTable: "koruxa_boss",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_koruxa_boss_attack_boss_id",
                table: "koruxa_boss_attack",
                column: "boss_id");

            migrationBuilder.CreateIndex(
                name: "ix_koruxa_user_discord_user_id",
                table: "koruxa_user",
                column: "discord_user_id",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "koruxa_boss_attack");

            migrationBuilder.DropTable(
                name: "koruxa_user");

            migrationBuilder.DropTable(
                name: "koruxa_boss");
        }
    }
}
