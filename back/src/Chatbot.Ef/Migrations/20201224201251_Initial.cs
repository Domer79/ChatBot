using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Chatbot.Ef.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "operator_logs",
                columns: table => new
                {
                    operator_log_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    operator_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    action = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    date_created = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_operator_logs", x => x.operator_log_id);
                });

            migrationBuilder.CreateTable(
                name: "permission",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    politic = table.Column<int>(type: "int", nullable: false),
                    date_created = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_permission", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "question_response",
                columns: table => new
                {
                    question_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    question = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    response = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    parent_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    date_created = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()")
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

            migrationBuilder.CreateTable(
                name: "role",
                columns: table => new
                {
                    role_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    date_created = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_role", x => x.role_id);
                });

            migrationBuilder.CreateTable(
                name: "user",
                columns: table => new
                {
                    user_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    login = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    email = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    first_name = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    last_name = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    middle_name = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    date_created = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    password = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    is_active = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user", x => x.user_id);
                });

            migrationBuilder.CreateTable(
                name: "role_permission",
                columns: table => new
                {
                    role_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    permission_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    date_created = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_role_permission", x => new { x.role_id, x.permission_id });
                    table.ForeignKey(
                        name: "FK_role_permission_permission_permission_id",
                        column: x => x.permission_id,
                        principalTable: "permission",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_role_permission_role_role_id",
                        column: x => x.role_id,
                        principalTable: "role",
                        principalColumn: "role_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "message_dialog",
                columns: table => new
                {
                    message_dialog_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    number = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    date_created = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    date_work = table.Column<DateTime>(type: "datetime2", nullable: true),
                    date_completed = table.Column<DateTime>(type: "datetime2", nullable: true),
                    dialog_status = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    operator_id = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    client_id = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_message_dialog", x => x.message_dialog_id);
                    table.ForeignKey(
                        name: "FK_message_dialog_user_operator_id",
                        column: x => x.operator_id,
                        principalTable: "user",
                        principalColumn: "user_id");
                });

            migrationBuilder.CreateTable(
                name: "token",
                columns: table => new
                {
                    token_id = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    date_created = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    date_expired = table.Column<DateTime>(type: "datetime2", nullable: false),
                    auto_expired = table.Column<TimeSpan>(type: "time", nullable: false),
                    user_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_token", x => x.token_id);
                    table.ForeignKey(
                        name: "FK_token_user_user_id",
                        column: x => x.user_id,
                        principalTable: "user",
                        principalColumn: "user_id");
                });

            migrationBuilder.CreateTable(
                name: "user_role",
                columns: table => new
                {
                    user_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    role_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    date_created = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_role", x => new { x.user_id, x.role_id });
                    table.ForeignKey(
                        name: "FK_user_role_role_role_id",
                        column: x => x.role_id,
                        principalTable: "role",
                        principalColumn: "role_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_user_role_user_user_id",
                        column: x => x.user_id,
                        principalTable: "user",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "message",
                columns: table => new
                {
                    message_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    type = table.Column<int>(type: "int", nullable: false),
                    owner = table.Column<int>(type: "int", nullable: false),
                    status = table.Column<int>(type: "int", nullable: false),
                    time = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    message_dialog_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    sender = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_message", x => x.message_id);
                    table.ForeignKey(
                        name: "FK_message_message_dialog_message_dialog_id",
                        column: x => x.message_dialog_id,
                        principalTable: "message_dialog",
                        principalColumn: "message_dialog_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_message_message_dialog_id",
                table: "message",
                column: "message_dialog_id");

            migrationBuilder.CreateIndex(
                name: "IX_message_dialog_number",
                table: "message_dialog",
                column: "number",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_message_dialog_operator_id",
                table: "message_dialog",
                column: "operator_id");

            migrationBuilder.CreateIndex(
                name: "UQ_permission_politic",
                table: "permission",
                column: "politic",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_question_response_parent_id",
                table: "question_response",
                column: "parent_id");

            migrationBuilder.CreateIndex(
                name: "IX_question_response_question",
                table: "question_response",
                column: "question",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_role_Name",
                table: "role",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_role_permission_permission_id",
                table: "role_permission",
                column: "permission_id");

            migrationBuilder.CreateIndex(
                name: "IX_token_user_id",
                table: "token",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_user_email",
                table: "user",
                column: "email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_user_login",
                table: "user",
                column: "login",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_user_role_role_id",
                table: "user_role",
                column: "role_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "message");

            migrationBuilder.DropTable(
                name: "operator_logs");

            migrationBuilder.DropTable(
                name: "question_response");

            migrationBuilder.DropTable(
                name: "role_permission");

            migrationBuilder.DropTable(
                name: "token");

            migrationBuilder.DropTable(
                name: "user_role");

            migrationBuilder.DropTable(
                name: "message_dialog");

            migrationBuilder.DropTable(
                name: "permission");

            migrationBuilder.DropTable(
                name: "role");

            migrationBuilder.DropTable(
                name: "user");
        }
    }
}
