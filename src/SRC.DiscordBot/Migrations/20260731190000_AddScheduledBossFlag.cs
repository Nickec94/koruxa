using Microsoft.EntityFrameworkCore.Migrations;

namespace SRC.DiscordBot.Migrations;

public partial class AddScheduledBossFlag : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<bool>(
            name: "is_scheduled",
            table: "koruxa_boss",
            type: "INTEGER",
            nullable: false,
            defaultValue: false);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "is_scheduled",
            table: "koruxa_boss");
    }
}
