using Microsoft.EntityFrameworkCore.Migrations;

namespace ProjectManagementTracketAPI.Migrations
{
    public partial class Add_Assigning_Task_Table_Modified : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AssigningTask_Members_MemberId",
                table: "AssigningTask");

            migrationBuilder.DropIndex(
                name: "IX_AssigningTask_MemberId",
                table: "AssigningTask");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_AssigningTask_MemberId",
                table: "AssigningTask",
                column: "MemberId");

            migrationBuilder.AddForeignKey(
                name: "FK_AssigningTask_Members_MemberId",
                table: "AssigningTask",
                column: "MemberId",
                principalTable: "Members",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
