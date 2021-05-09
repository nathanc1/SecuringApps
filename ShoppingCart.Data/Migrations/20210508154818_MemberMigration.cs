using Microsoft.EntityFrameworkCore.Migrations;

namespace ShoppingCart.Data.Migrations
{
    public partial class MemberMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "privateKey",
                table: "Members",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "publicKey",
                table: "Members",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "teacherEmail",
                table: "Members",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "privateKey",
                table: "Members");

            migrationBuilder.DropColumn(
                name: "publicKey",
                table: "Members");

            migrationBuilder.DropColumn(
                name: "teacherEmail",
                table: "Members");
        }
    }
}
