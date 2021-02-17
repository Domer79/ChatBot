using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Chatbot.Ef.Migrations
{
    public partial class _16 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "based_id",
                table: "message_dialog",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_message_dialog_based_id",
                table: "message_dialog",
                column: "based_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_message_dialog_based_id",
                table: "message_dialog");

            migrationBuilder.DropColumn(
                name: "based_id",
                table: "message_dialog");
        }
    }
}
