using Microsoft.EntityFrameworkCore.Migrations;

namespace ProjectManagementTracketAPI.Migrations
{
    public partial class Add_Column : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MemberId",
                table: "Members",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MemberId",
                table: "Members");
        }
    }
}
