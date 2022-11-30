using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MemberAssigendTask.Migrations
{
    public partial class Added_Table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AssigningTask",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MemberId = table.Column<int>(nullable: false),
                    MemberName = table.Column<string>(nullable: false),
                    TaskName = table.Column<string>(nullable: false),
                    Deliverbles = table.Column<string>(nullable: false),
                    TaskStartDate = table.Column<DateTime>(nullable: false),
                    TaskEndDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssigningTask", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AssigningTask");
        }
    }
}
