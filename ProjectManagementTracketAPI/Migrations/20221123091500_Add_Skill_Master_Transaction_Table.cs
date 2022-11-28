using Microsoft.EntityFrameworkCore.Migrations;

namespace ProjectManagementTracketAPI.Migrations
{
    public partial class Add_Skill_Master_Transaction_Table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SkillSet",
                table: "Members");

            migrationBuilder.CreateTable(
                name: "SkillsMaster",
                columns: table => new
                {
                    SkillId = table.Column<short>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SkillName = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SkillsMaster", x => x.SkillId);
                });

            migrationBuilder.CreateTable(
                name: "SkillsTransaction",
                columns: table => new
                {
                    MemberId = table.Column<int>(nullable: false),
                    SkillId = table.Column<short>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SkillsTransaction", x => new { x.SkillId, x.MemberId });
                    table.ForeignKey(
                        name: "FK_SkillsTransaction_SkillsMaster_SkillId",
                        column: x => x.SkillId,
                        principalTable: "SkillsMaster",
                        principalColumn: "SkillId",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SkillsTransaction");

            migrationBuilder.DropTable(
                name: "SkillsMaster");

            migrationBuilder.AddColumn<short>(
                name: "SkillSet",
                table: "Members",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0);
        }
    }
}
