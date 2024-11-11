using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace TestToeic.Migrations
{
    /// <inheritdoc />
    public partial class updateTable811 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AnswerOfStudents_AspNetUsers_ApplicationUserId",
                table: "AnswerOfStudents");

            migrationBuilder.DropForeignKey(
                name: "FK_AnswerOfStudents_StudentPoints_StudentPointStudentAnswerId",
                table: "AnswerOfStudents");

            migrationBuilder.DropForeignKey(
                name: "FK_AnswerOfStudents_Tests_TestId",
                table: "AnswerOfStudents");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentPoints_AspNetUsers_ApplicationUserId",
                table: "StudentPoints");

            migrationBuilder.DropPrimaryKey(
                name: "PK_StudentPoints",
                table: "StudentPoints");

            migrationBuilder.DropIndex(
                name: "IX_AnswerOfStudents_ApplicationUserId",
                table: "AnswerOfStudents");

            migrationBuilder.DropIndex(
                name: "IX_AnswerOfStudents_StudentPointStudentAnswerId",
                table: "AnswerOfStudents");

            migrationBuilder.DropColumn(
                name: "StudentAnswerPoint",
                table: "StudentPoints");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "AnswerOfStudents");

            migrationBuilder.DropColumn(
                name: "StudentPointStudentAnswerId",
                table: "AnswerOfStudents");

            migrationBuilder.RenameColumn(
                name: "StudentAnswerId",
                table: "StudentPoints",
                newName: "TestId");

            migrationBuilder.RenameColumn(
                name: "TestId",
                table: "AnswerOfStudents",
                newName: "StudentPointId");

            migrationBuilder.RenameIndex(
                name: "IX_AnswerOfStudents_TestId",
                table: "AnswerOfStudents",
                newName: "IX_AnswerOfStudents_StudentPointId");

            migrationBuilder.AlterColumn<string>(
                name: "ApplicationUserId",
                table: "StudentPoints",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "TestId",
                table: "StudentPoints",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddColumn<int>(
                name: "StudentPointId",
                table: "StudentPoints",
                type: "integer",
                nullable: false,
                defaultValue: 0)
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddPrimaryKey(
                name: "PK_StudentPoints",
                table: "StudentPoints",
                column: "StudentPointId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentPoints_TestId",
                table: "StudentPoints",
                column: "TestId");

            migrationBuilder.AddForeignKey(
                name: "FK_AnswerOfStudents_StudentPoints_StudentPointId",
                table: "AnswerOfStudents",
                column: "StudentPointId",
                principalTable: "StudentPoints",
                principalColumn: "StudentPointId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StudentPoints_AspNetUsers_ApplicationUserId",
                table: "StudentPoints",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StudentPoints_Tests_TestId",
                table: "StudentPoints",
                column: "TestId",
                principalTable: "Tests",
                principalColumn: "TestId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AnswerOfStudents_StudentPoints_StudentPointId",
                table: "AnswerOfStudents");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentPoints_AspNetUsers_ApplicationUserId",
                table: "StudentPoints");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentPoints_Tests_TestId",
                table: "StudentPoints");

            migrationBuilder.DropPrimaryKey(
                name: "PK_StudentPoints",
                table: "StudentPoints");

            migrationBuilder.DropIndex(
                name: "IX_StudentPoints_TestId",
                table: "StudentPoints");

            migrationBuilder.DropColumn(
                name: "StudentPointId",
                table: "StudentPoints");

            migrationBuilder.RenameColumn(
                name: "TestId",
                table: "StudentPoints",
                newName: "StudentAnswerId");

            migrationBuilder.RenameColumn(
                name: "StudentPointId",
                table: "AnswerOfStudents",
                newName: "TestId");

            migrationBuilder.RenameIndex(
                name: "IX_AnswerOfStudents_StudentPointId",
                table: "AnswerOfStudents",
                newName: "IX_AnswerOfStudents_TestId");

            migrationBuilder.AlterColumn<string>(
                name: "ApplicationUserId",
                table: "StudentPoints",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<int>(
                name: "StudentAnswerId",
                table: "StudentPoints",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddColumn<float>(
                name: "StudentAnswerPoint",
                table: "StudentPoints",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "AnswerOfStudents",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "StudentPointStudentAnswerId",
                table: "AnswerOfStudents",
                type: "integer",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_StudentPoints",
                table: "StudentPoints",
                column: "StudentAnswerId");

            migrationBuilder.CreateIndex(
                name: "IX_AnswerOfStudents_ApplicationUserId",
                table: "AnswerOfStudents",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_AnswerOfStudents_StudentPointStudentAnswerId",
                table: "AnswerOfStudents",
                column: "StudentPointStudentAnswerId");

            migrationBuilder.AddForeignKey(
                name: "FK_AnswerOfStudents_AspNetUsers_ApplicationUserId",
                table: "AnswerOfStudents",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AnswerOfStudents_StudentPoints_StudentPointStudentAnswerId",
                table: "AnswerOfStudents",
                column: "StudentPointStudentAnswerId",
                principalTable: "StudentPoints",
                principalColumn: "StudentAnswerId");

            migrationBuilder.AddForeignKey(
                name: "FK_AnswerOfStudents_Tests_TestId",
                table: "AnswerOfStudents",
                column: "TestId",
                principalTable: "Tests",
                principalColumn: "TestId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StudentPoints_AspNetUsers_ApplicationUserId",
                table: "StudentPoints",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
