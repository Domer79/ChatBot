using Microsoft.EntityFrameworkCore.Migrations;

namespace Chatbot.Ef.Migrations
{
    public partial class _5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "UQ_permission_politic",
                table: "permission");

            migrationBuilder.CreateIndex(
                name: "IX_role_Name",
                table: "role",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ_permission_politic",
                table: "permission",
                column: "politic",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_role_Name",
                table: "role");

            migrationBuilder.DropIndex(
                name: "UQ_permission_politic",
                table: "permission");

            migrationBuilder.CreateIndex(
                name: "UQ_permission_politic",
                table: "permission",
                column: "politic");
        }
    }
}
