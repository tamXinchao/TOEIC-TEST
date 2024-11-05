using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TestToeic.Migrations
{
    /// <inheritdoc />
    public partial class updateTest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "QuestionId",
                table: "Tests",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Tests_QuestionId",
                table: "Tests",
                column: "QuestionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tests_Questions_QuestionId",
                table: "Tests",
                column: "QuestionId",
                principalTable: "Questions",
                principalColumn: "QuestionId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tests_Questions_QuestionId",
                table: "Tests");

            migrationBuilder.DropIndex(
                name: "IX_Tests_QuestionId",
                table: "Tests");

            migrationBuilder.DropColumn(
                name: "QuestionId",
                table: "Tests");
        }
    }
}
