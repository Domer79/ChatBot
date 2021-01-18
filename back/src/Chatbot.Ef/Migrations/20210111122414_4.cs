using Microsoft.EntityFrameworkCore.Migrations;

namespace Chatbot.Ef.Migrations
{
    public partial class _4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "is_operator",
                table: "user",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_user_is_operator",
                table: "user",
                column: "is_operator");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_user_is_operator",
                table: "user");

            migrationBuilder.DropColumn(
                name: "is_operator",
                table: "user");
        }
    }
}
