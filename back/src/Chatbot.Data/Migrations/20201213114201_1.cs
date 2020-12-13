using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Chatbot.Data.Migrations
{
    public partial class _1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "question_response",
                columns: table => new
                {
                    question_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    question = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    response = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    parent_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_question_response", x => x.question_id);
                    table.ForeignKey(
                        name: "FK_question_response_question_response_parent_id",
                        column: x => x.parent_id,
                        principalTable: "question_response",
                        principalColumn: "question_id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_question_response_parent_id",
                table: "question_response",
                column: "parent_id");

            migrationBuilder.CreateIndex(
                name: "IX_question_response_question",
                table: "question_response",
                column: "question",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "question_response");
        }
    }
}
