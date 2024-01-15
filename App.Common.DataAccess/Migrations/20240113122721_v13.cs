using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace App.Common.DataAccess.Migrations
{
    public partial class v13 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CompanySavedCvs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CvId = table.Column<int>(type: "int", nullable: false),
                    DepartmentId = table.Column<int>(type: "int", nullable: true),
                    CompanyId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanySavedCvs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CompanySavedCvs_CompanyDepartments_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "CompanyDepartments",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CompanySavedCvs_CompanyInfos_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "CompanyInfos",
                        principalColumn: "CompanyInfoId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CompanySavedCvs_UserCVs_CvId",
                        column: x => x.CvId,
                        principalTable: "UserCVs",
                        principalColumn: "CVId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CompanySavedCvs_CompanyId",
                table: "CompanySavedCvs",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_CompanySavedCvs_CvId",
                table: "CompanySavedCvs",
                column: "CvId");

            migrationBuilder.CreateIndex(
                name: "IX_CompanySavedCvs_DepartmentId",
                table: "CompanySavedCvs",
                column: "DepartmentId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CompanySavedCvs");
        }
    }
}
