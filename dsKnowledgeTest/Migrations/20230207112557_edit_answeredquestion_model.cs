using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace dsKnowledgeTest.Migrations
{
    public partial class edit_answeredquestion_model : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PassedTestsId",
                table: "AnsweredQuestions");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "PassedTestsId",
                table: "AnsweredQuestions",
                type: "uniqueidentifier",
                nullable: true);
        }
    }
}
