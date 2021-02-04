using Microsoft.EntityFrameworkCore.Migrations;

namespace Chatbot.Ef.Migrations
{
    public partial class _9 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_question_response_question",
                table: "question_response");

            migrationBuilder.AlterColumn<string>(
                name: "question",
                table: "question_response",
                type: "nvarchar(800)",
                maxLength: 800,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.CreateIndex(
                name: "IX_question_response_question",
                table: "question_response",
                column: "question");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_question_response_question",
                table: "question_response");

            migrationBuilder.AlterColumn<string>(
                name: "question",
                table: "question_response",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(800)",
                oldMaxLength: 800);

            migrationBuilder.CreateIndex(
                name: "IX_question_response_question",
                table: "question_response",
                column: "question",
                unique: true);
        }
    }
}
