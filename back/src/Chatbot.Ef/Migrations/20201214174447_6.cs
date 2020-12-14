using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Chatbot.Ef.Migrations
{
    public partial class _6 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_message_dialog_user_client_id",
                table: "message_dialog");

            migrationBuilder.DropForeignKey(
                name: "FK_message_dialog_user_operator_id",
                table: "message_dialog");

            migrationBuilder.DropIndex(
                name: "IX_message_dialog_client_id",
                table: "message_dialog");

            migrationBuilder.DropIndex(
                name: "IX_message_dialog_operator_id",
                table: "message_dialog");

            migrationBuilder.DropColumn(
                name: "client_id",
                table: "message_dialog");

            migrationBuilder.DropColumn(
                name: "operator_id",
                table: "message_dialog");

            migrationBuilder.AddColumn<Guid>(
                name: "sender",
                table: "message",
                type: "uniqueidentifier",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "sender",
                table: "message");

            migrationBuilder.AddColumn<Guid>(
                name: "client_id",
                table: "message_dialog",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "operator_id",
                table: "message_dialog",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_message_dialog_client_id",
                table: "message_dialog",
                column: "client_id");

            migrationBuilder.CreateIndex(
                name: "IX_message_dialog_operator_id",
                table: "message_dialog",
                column: "operator_id");

            migrationBuilder.AddForeignKey(
                name: "FK_message_dialog_user_client_id",
                table: "message_dialog",
                column: "client_id",
                principalTable: "user",
                principalColumn: "user_id");

            migrationBuilder.AddForeignKey(
                name: "FK_message_dialog_user_operator_id",
                table: "message_dialog",
                column: "operator_id",
                principalTable: "user",
                principalColumn: "user_id");
        }
    }
}
