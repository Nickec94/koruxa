using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SRC.DiscordBot.Migrations
{
    /// <inheritdoc />
    public partial class AddNotifiedFlag : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "has_notified_spawn",
                table: "koruxa_boss",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "has_notified_spawn",
                table: "koruxa_boss");
        }
    }
}
