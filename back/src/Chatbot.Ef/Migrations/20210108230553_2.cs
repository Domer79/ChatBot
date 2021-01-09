using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Chatbot.Ef.Migrations
{
    public partial class _2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Guid>(
                name: "parent_id",
                table: "question_response",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.CreateIndex(
                name: "IX_message_dialog_client_id",
                table: "message_dialog",
                column: "client_id");

            migrationBuilder.AddForeignKey(
                name: "FK_message_dialog_user_client_id",
                table: "message_dialog",
                column: "client_id",
                principalTable: "user",
                principalColumn: "user_id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_message_dialog_user_client_id",
                table: "message_dialog");

            migrationBuilder.DropIndex(
                name: "IX_message_dialog_client_id",
                table: "message_dialog");

            migrationBuilder.AlterColumn<Guid>(
                name: "parent_id",
                table: "question_response",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);
        }
    }
}
