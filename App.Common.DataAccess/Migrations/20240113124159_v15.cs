using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace App.Common.DataAccess.Migrations
{
    public partial class v15 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CompanySavedCvs_CompanyInfos_CompanyInfoId",
                table: "CompanySavedCvs");

            migrationBuilder.DropIndex(
                name: "IX_CompanySavedCvs_CompanyInfoId",
                table: "CompanySavedCvs");

            migrationBuilder.DropColumn(
                name: "CompanyInfoId",
                table: "CompanySavedCvs");

            migrationBuilder.CreateIndex(
                name: "IX_CompanySavedCvs_CompanyId",
                table: "CompanySavedCvs",
                column: "CompanyId");

            migrationBuilder.AddForeignKey(
                name: "FK_CompanySavedCvs_Companies_CompanyId",
                table: "CompanySavedCvs",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "CompanyId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CompanySavedCvs_Companies_CompanyId",
                table: "CompanySavedCvs");

            migrationBuilder.DropIndex(
                name: "IX_CompanySavedCvs_CompanyId",
                table: "CompanySavedCvs");

            migrationBuilder.AddColumn<int>(
                name: "CompanyInfoId",
                table: "CompanySavedCvs",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_CompanySavedCvs_CompanyInfoId",
                table: "CompanySavedCvs",
                column: "CompanyInfoId");

            migrationBuilder.AddForeignKey(
                name: "FK_CompanySavedCvs_CompanyInfos_CompanyInfoId",
                table: "CompanySavedCvs",
                column: "CompanyInfoId",
                principalTable: "CompanyInfos",
                principalColumn: "CompanyInfoId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
