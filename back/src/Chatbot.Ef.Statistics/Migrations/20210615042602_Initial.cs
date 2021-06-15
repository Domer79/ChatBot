using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Chatbot.Ef.Statistics.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Stat",
                columns: table => new
                {
                    stat_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    user_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    question_id = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    time = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    stat_type = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stat", x => x.stat_id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Stat_question_id",
                table: "Stat",
                column: "question_id");

            migrationBuilder.CreateIndex(
                name: "IX_Stat_stat_type",
                table: "Stat",
                column: "stat_type");

            migrationBuilder.CreateIndex(
                name: "IX_Stat_time",
                table: "Stat",
                column: "time",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Stat_user_id",
                table: "Stat",
                column: "user_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Stat");
        }
    }
}
