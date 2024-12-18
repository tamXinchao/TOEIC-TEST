using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TestToeic.Migrations
{
    /// <inheritdoc />
    public partial class RemoveClassIdFromTest2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // migrationBuilder.DropForeignKey(
            //     name: "FK_Tests_Classes_ClassId",
            //     table: "Tests");

            // migrationBuilder.DropIndex(
            //     name: "IX_Tests_ClassId",
            //     table: "Tests");
            //
            // migrationBuilder.DropColumn(
            //     name: "ClassId",
            //     table: "Tests");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ClassId",
                table: "Tests",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tests_ClassId",
                table: "Tests",
                column: "ClassId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tests_Classes_ClassId",
                table: "Tests",
                column: "ClassId",
                principalTable: "Classes",
                principalColumn: "ClassId");
        }
    }
}
