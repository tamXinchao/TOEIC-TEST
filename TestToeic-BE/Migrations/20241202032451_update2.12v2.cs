using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TestToeic.Migrations
{
    /// <inheritdoc />
    public partial class update212v2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "StartPoint",
                table: "Notices",
                newName: "ScoreMin");

            migrationBuilder.RenameColumn(
                name: "EndPoint",
                table: "Notices",
                newName: "ScoreMax");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ScoreMin",
                table: "Notices",
                newName: "StartPoint");

            migrationBuilder.RenameColumn(
                name: "ScoreMax",
                table: "Notices",
                newName: "EndPoint");
        }
    }
}
