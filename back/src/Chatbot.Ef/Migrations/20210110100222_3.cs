using Microsoft.EntityFrameworkCore.Migrations;

namespace Chatbot.Ef.Migrations
{
    public partial class _3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "number",
                table: "question_response",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.CreateIndex(
                name: "IX_question_response_number",
                table: "question_response",
                column: "number",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_question_response_number",
                table: "question_response");

            migrationBuilder.DropColumn(
                name: "number",
                table: "question_response");
        }
    }
}
