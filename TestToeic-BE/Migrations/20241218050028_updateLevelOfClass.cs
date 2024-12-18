using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace TestToeic.Migrations
{
    /// <inheritdoc />
    public partial class updateLevelOfClass : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LevelOfClassId",
                table: "Classes",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "LevelOfClasses",
                columns: table => new
                {
                    LevelOfClassId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    LevelName = table.Column<string>(type: "text", nullable: false),
                    IsDelete = table.Column<bool>(type: "boolean", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LevelOfClasses", x => x.LevelOfClassId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Classes_LevelOfClassId",
                table: "Classes",
                column: "LevelOfClassId");

            migrationBuilder.AddForeignKey(
                name: "FK_Classes_LevelOfClasses_LevelOfClassId",
                table: "Classes",
                column: "LevelOfClassId",
                principalTable: "LevelOfClasses",
                principalColumn: "LevelOfClassId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Classes_LevelOfClasses_LevelOfClassId",
                table: "Classes");

            migrationBuilder.DropTable(
                name: "LevelOfClasses");

            migrationBuilder.DropIndex(
                name: "IX_Classes_LevelOfClassId",
                table: "Classes");

            migrationBuilder.DropColumn(
                name: "LevelOfClassId",
                table: "Classes");
        }
    }
}
