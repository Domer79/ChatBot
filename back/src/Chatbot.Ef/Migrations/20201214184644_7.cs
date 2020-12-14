using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Chatbot.Ef.Migrations
{
    public partial class _7 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "dialog_status",
                table: "message_dialog",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AlterColumn<DateTime>(
                name: "time",
                table: "message",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETDATE()",
                oldClrType: typeof(DateTime),
                oldType: "datetime2");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "dialog_status",
                table: "message_dialog");

            migrationBuilder.AlterColumn<DateTime>(
                name: "time",
                table: "message",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValueSql: "GETDATE()");
        }
    }
}
