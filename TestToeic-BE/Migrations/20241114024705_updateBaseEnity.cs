using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TestToeic.Migrations
{
    /// <inheritdoc />
    public partial class updateBaseEnity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Tests",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDelete",
                table: "Tests",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "StudentPoints",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDelete",
                table: "StudentPoints",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Schedules",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDelete",
                table: "Schedules",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Questions",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDelete",
                table: "Questions",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "PointOfQuestions",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDelete",
                table: "PointOfQuestions",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Classes",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDelete",
                table: "Classes",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Answers",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDelete",
                table: "Answers",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "AnswerOfStudents",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDelete",
                table: "AnswerOfStudents",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Tests");

            migrationBuilder.DropColumn(
                name: "IsDelete",
                table: "Tests");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "StudentPoints");

            migrationBuilder.DropColumn(
                name: "IsDelete",
                table: "StudentPoints");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Schedules");

            migrationBuilder.DropColumn(
                name: "IsDelete",
                table: "Schedules");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Questions");

            migrationBuilder.DropColumn(
                name: "IsDelete",
                table: "Questions");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "PointOfQuestions");

            migrationBuilder.DropColumn(
                name: "IsDelete",
                table: "PointOfQuestions");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Classes");

            migrationBuilder.DropColumn(
                name: "IsDelete",
                table: "Classes");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Answers");

            migrationBuilder.DropColumn(
                name: "IsDelete",
                table: "Answers");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "AnswerOfStudents");

            migrationBuilder.DropColumn(
                name: "IsDelete",
                table: "AnswerOfStudents");
        }
    }
}
