using Microsoft.EntityFrameworkCore.Migrations;

namespace Chatbot.Ef.Migrations
{
    public partial class _10 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_OperatorLogs",
                table: "OperatorLogs");

            migrationBuilder.RenameTable(
                name: "OperatorLogs",
                newName: "operator_logs");

            migrationBuilder.AddPrimaryKey(
                name: "PK_operator_logs",
                table: "operator_logs",
                column: "operator_log_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_operator_logs",
                table: "operator_logs");

            migrationBuilder.RenameTable(
                name: "operator_logs",
                newName: "OperatorLogs");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OperatorLogs",
                table: "OperatorLogs",
                column: "operator_log_id");
        }
    }
}
