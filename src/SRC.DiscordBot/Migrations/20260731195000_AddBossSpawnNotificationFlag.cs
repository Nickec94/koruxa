using Microsoft.EntityFrameworkCore.Migrations;

namespace SRC.DiscordBot.Migrations;

public partial class AddBossSpawnNotificationFlag : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<bool>(
            name: "has_notified_spawn",
            table: "koruxa_boss",
            type: "INTEGER",
            nullable: false,
            defaultValue: false);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "has_notified_spawn",
            table: "koruxa_boss");
    }
}
