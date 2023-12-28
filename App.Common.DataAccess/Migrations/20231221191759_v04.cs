using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace App.Common.DataAccess.Migrations
{
    public partial class v04 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "City",
                table: "UserCVs");

            migrationBuilder.DropColumn(
                name: "Country",
                table: "UserCVs");

            migrationBuilder.AlterColumn<string>(
                name: "Image",
                table: "UserCVs",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Image",
                table: "UserCVs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "UserCVs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Country",
                table: "UserCVs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
