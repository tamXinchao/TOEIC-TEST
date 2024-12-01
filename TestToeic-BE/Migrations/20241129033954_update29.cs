using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace TestToeic.Migrations
{
    /// <inheritdoc />
    public partial class update29 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "MultipleAnswer",
                table: "Questions",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "ParentQuestionId",
                table: "Questions",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Primary",
                table: "Questions",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "AnswerContent",
                table: "AnswerOfStudents",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Note",
                table: "AnswerOfStudents",
                type: "text",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Stickers",
                columns: table => new
                {
                    StickerId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    StickerName = table.Column<string>(type: "text", nullable: false),
                    Note = table.Column<string>(type: "text", nullable: true),
                    IsDelete = table.Column<bool>(type: "boolean", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stickers", x => x.StickerId);
                });

            migrationBuilder.CreateTable(
                name: "StickerOfTests",
                columns: table => new
                {
                    StickerOfTestId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TestId = table.Column<int>(type: "integer", nullable: false),
                    StickerId = table.Column<int>(type: "integer", nullable: false),
                    IsDelete = table.Column<bool>(type: "boolean", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StickerOfTests", x => x.StickerOfTestId);
                    table.ForeignKey(
                        name: "FK_StickerOfTests_Stickers_StickerId",
                        column: x => x.StickerId,
                        principalTable: "Stickers",
                        principalColumn: "StickerId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StickerOfTests_Tests_TestId",
                        column: x => x.TestId,
                        principalTable: "Tests",
                        principalColumn: "TestId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Questions_ParentQuestionId",
                table: "Questions",
                column: "ParentQuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_StickerOfTests_StickerId",
                table: "StickerOfTests",
                column: "StickerId");

            migrationBuilder.CreateIndex(
                name: "IX_StickerOfTests_TestId",
                table: "StickerOfTests",
                column: "TestId");

            migrationBuilder.AddForeignKey(
                name: "FK_Questions_Questions_ParentQuestionId",
                table: "Questions",
                column: "ParentQuestionId",
                principalTable: "Questions",
                principalColumn: "QuestionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Questions_Questions_ParentQuestionId",
                table: "Questions");

            migrationBuilder.DropTable(
                name: "StickerOfTests");

            migrationBuilder.DropTable(
                name: "Stickers");

            migrationBuilder.DropIndex(
                name: "IX_Questions_ParentQuestionId",
                table: "Questions");

            migrationBuilder.DropColumn(
                name: "MultipleAnswer",
                table: "Questions");

            migrationBuilder.DropColumn(
                name: "ParentQuestionId",
                table: "Questions");

            migrationBuilder.DropColumn(
                name: "Primary",
                table: "Questions");

            migrationBuilder.DropColumn(
                name: "AnswerContent",
                table: "AnswerOfStudents");

            migrationBuilder.DropColumn(
                name: "Note",
                table: "AnswerOfStudents");
        }
    }
}
