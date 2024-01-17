using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace App.Common.DataAccess.Migrations
{
    public partial class v21 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "MeetingSubject",
                table: "CompanySavedCvs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<TimeSpan>(
                name: "MeetingTime",
                table: "CompanySavedCvs",
                type: "time",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MeetingSubject",
                table: "CompanySavedCvs");

            migrationBuilder.DropColumn(
                name: "MeetingTime",
                table: "CompanySavedCvs");
        }
    }
}
