using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace dsKnowledgeTest.Migrations
{
    public partial class edit_test_and_question_model : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsRandomAnswers",
                table: "Tests",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsRandomQuestions",
                table: "Tests",
                type: "bit",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "TrueAnswers",
                table: "Questions",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Answers",
                table: "Questions",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsRandomAnswers",
                table: "Tests");

            migrationBuilder.DropColumn(
                name: "IsRandomQuestions",
                table: "Tests");

            migrationBuilder.AlterColumn<string>(
                name: "TrueAnswers",
                table: "Questions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Answers",
                table: "Questions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }
    }
}
