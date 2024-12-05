using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TestToeic.Migrations
{
    /// <inheritdoc />
    public partial class RemoveClassIdFromTests : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Xóa cột ClassId khỏi bảng Tests
            migrationBuilder.DropForeignKey(
                name: "FK_Tests_Classes_ClassId",
                table: "Tests");

            migrationBuilder.DropColumn(
                name: "ClassId",
                table: "Tests");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
// Nếu rollback, thêm lại cột ClassId và foreign key
            migrationBuilder.AddColumn<int>(
                name: "ClassId",
                table: "Tests",
                type: "integer",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Tests_Classes_ClassId",
                table: "Tests",
                column: "ClassId",
                principalTable: "Classes",
                principalColumn: "ClassId");

        }
    }
}
