using Microsoft.EntityFrameworkCore.Migrations;

namespace Chatbot.Ef.Migrations
{
    public partial class _11 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "number",
                table: "message_dialog",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.CreateIndex(
                name: "IX_message_dialog_number",
                table: "message_dialog",
                column: "number",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_message_dialog_number",
                table: "message_dialog");

            migrationBuilder.DropColumn(
                name: "number",
                table: "message_dialog");
        }
    }
}
