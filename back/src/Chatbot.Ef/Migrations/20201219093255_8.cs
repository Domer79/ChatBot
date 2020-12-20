using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Chatbot.Ef.Migrations
{
    public partial class _8 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "date_work",
                table: "message_dialog",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTime>(
                name: "date_completed",
                table: "message_dialog",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<Guid>(
                name: "operator_id",
                table: "message_dialog",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_message_dialog_operator_id",
                table: "message_dialog",
                column: "operator_id");

            migrationBuilder.AddForeignKey(
                name: "FK_message_dialog_user_operator_id",
                table: "message_dialog",
                column: "operator_id",
                principalTable: "user",
                principalColumn: "user_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_message_dialog_user_operator_id",
                table: "message_dialog");

            migrationBuilder.DropIndex(
                name: "IX_message_dialog_operator_id",
                table: "message_dialog");

            migrationBuilder.DropColumn(
                name: "operator_id",
                table: "message_dialog");

            migrationBuilder.AlterColumn<DateTime>(
                name: "date_work",
                table: "message_dialog",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "date_completed",
                table: "message_dialog",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);
        }
    }
}
