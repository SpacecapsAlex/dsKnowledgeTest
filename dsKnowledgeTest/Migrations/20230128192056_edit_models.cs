using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace dsKnowledgeTest.Migrations
{
    public partial class edit_models : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CourseNumber",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Group",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "TimeForTest",
                table: "Tests");

            migrationBuilder.RenameColumn(
                name: "Institution",
                table: "Users",
                newName: "Organization");

            migrationBuilder.AddColumn<int>(
                name: "Score",
                table: "Tests",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TimeTest",
                table: "Tests",
                type: "int",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Score",
                table: "Tests");

            migrationBuilder.DropColumn(
                name: "TimeTest",
                table: "Tests");

            migrationBuilder.RenameColumn(
                name: "Organization",
                table: "Users",
                newName: "Institution");

            migrationBuilder.AddColumn<int>(
                name: "CourseNumber",
                table: "Users",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Group",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<TimeSpan>(
                name: "TimeForTest",
                table: "Tests",
                type: "time",
                nullable: true);
        }
    }
}
