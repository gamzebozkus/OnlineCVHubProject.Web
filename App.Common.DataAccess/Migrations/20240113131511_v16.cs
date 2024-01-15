using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace App.Common.DataAccess.Migrations
{
    public partial class v16 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CompanySavedCvs_Companies_CompanyId",
                table: "CompanySavedCvs");

            migrationBuilder.DropIndex(
                name: "IX_CompanySavedCvs_CompanyId",
                table: "CompanySavedCvs");

            migrationBuilder.AlterColumn<string>(
                name: "CompanyId",
                table: "CompanySavedCvs",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "CompanyId1",
                table: "CompanySavedCvs",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_CompanySavedCvs_CompanyId1",
                table: "CompanySavedCvs",
                column: "CompanyId1");

            migrationBuilder.AddForeignKey(
                name: "FK_CompanySavedCvs_Companies_CompanyId1",
                table: "CompanySavedCvs",
                column: "CompanyId1",
                principalTable: "Companies",
                principalColumn: "CompanyId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CompanySavedCvs_Companies_CompanyId1",
                table: "CompanySavedCvs");

            migrationBuilder.DropIndex(
                name: "IX_CompanySavedCvs_CompanyId1",
                table: "CompanySavedCvs");

            migrationBuilder.DropColumn(
                name: "CompanyId1",
                table: "CompanySavedCvs");

            migrationBuilder.AlterColumn<int>(
                name: "CompanyId",
                table: "CompanySavedCvs",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

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
    }
}
