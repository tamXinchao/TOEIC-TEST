using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TestToeic.Migrations
{
    /// <inheritdoc />
    public partial class updateQuestion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "LabelOfPrimaryQuestion",
                table: "Questions",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LabelOfPrimaryQuestion",
                table: "Questions");
        }
    }
}
