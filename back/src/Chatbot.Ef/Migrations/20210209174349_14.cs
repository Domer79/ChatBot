using Microsoft.EntityFrameworkCore.Migrations;

namespace Chatbot.Ef.Migrations
{
    public partial class _14 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "fio",
                table: "user",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_user_fio",
                table: "user",
                column: "fio");

            migrationBuilder.Sql(@"
update [User] set fio = temp_user.fio
from (select user_id id1, (last_name + ' ' + first_name + ' ' + middle_name) as fio from [User]) temp_user
where user_id = temp_user.id1
");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_user_fio",
                table: "user");

            migrationBuilder.DropColumn(
                name: "fio",
                table: "user");
        }
    }
}
