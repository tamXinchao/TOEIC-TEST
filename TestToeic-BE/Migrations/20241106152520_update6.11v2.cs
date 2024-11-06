using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace TestToeic.Migrations
{
    /// <inheritdoc />
    public partial class update611v2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PointOfQuestion_Questions_QuestionId",
                table: "PointOfQuestion");

            migrationBuilder.DropForeignKey(
                name: "FK_PointOfQuestion_Tests_TestId",
                table: "PointOfQuestion");

            migrationBuilder.DropTable(
                name: "StudentAnswers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PointOfQuestion",
                table: "PointOfQuestion");

            migrationBuilder.RenameTable(
                name: "PointOfQuestion",
                newName: "PointOfQuestions");

            migrationBuilder.RenameColumn(
                name: "Point",
                table: "Tests",
                newName: "PointOfTest");

            migrationBuilder.RenameIndex(
                name: "IX_PointOfQuestion_TestId",
                table: "PointOfQuestions",
                newName: "IX_PointOfQuestions_TestId");

            migrationBuilder.RenameIndex(
                name: "IX_PointOfQuestion_QuestionId",
                table: "PointOfQuestions",
                newName: "IX_PointOfQuestions_QuestionId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PointOfQuestions",
                table: "PointOfQuestions",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "StudentPoints",
                columns: table => new
                {
                    StudentAnswerId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    StudentAnswerPoint = table.Column<float>(type: "real", nullable: false),
                    Completion = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Duration = table.Column<TimeOnly>(type: "time without time zone", nullable: true),
                    PointOfStudent = table.Column<float>(type: "real", nullable: true),
                    ApplicationUserId = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentPoints", x => x.StudentAnswerId);
                    table.ForeignKey(
                        name: "FK_StudentPoints_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "AnswerOfStudents",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TestId = table.Column<int>(type: "integer", nullable: false),
                    AnswerId = table.Column<int>(type: "integer", nullable: false),
                    QuestionId = table.Column<int>(type: "integer", nullable: false),
                    ApplicationUserId = table.Column<string>(type: "text", nullable: false),
                    PointOfAnswer = table.Column<float>(type: "real", nullable: true),
                    StudentPointStudentAnswerId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnswerOfStudents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AnswerOfStudents_Answers_AnswerId",
                        column: x => x.AnswerId,
                        principalTable: "Answers",
                        principalColumn: "AnswerId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AnswerOfStudents_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AnswerOfStudents_Questions_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "Questions",
                        principalColumn: "QuestionId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AnswerOfStudents_StudentPoints_StudentPointStudentAnswerId",
                        column: x => x.StudentPointStudentAnswerId,
                        principalTable: "StudentPoints",
                        principalColumn: "StudentAnswerId");
                    table.ForeignKey(
                        name: "FK_AnswerOfStudents_Tests_TestId",
                        column: x => x.TestId,
                        principalTable: "Tests",
                        principalColumn: "TestId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AnswerOfStudents_AnswerId",
                table: "AnswerOfStudents",
                column: "AnswerId");

            migrationBuilder.CreateIndex(
                name: "IX_AnswerOfStudents_ApplicationUserId",
                table: "AnswerOfStudents",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_AnswerOfStudents_QuestionId",
                table: "AnswerOfStudents",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_AnswerOfStudents_StudentPointStudentAnswerId",
                table: "AnswerOfStudents",
                column: "StudentPointStudentAnswerId");

            migrationBuilder.CreateIndex(
                name: "IX_AnswerOfStudents_TestId",
                table: "AnswerOfStudents",
                column: "TestId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentPoints_ApplicationUserId",
                table: "StudentPoints",
                column: "ApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_PointOfQuestions_Questions_QuestionId",
                table: "PointOfQuestions",
                column: "QuestionId",
                principalTable: "Questions",
                principalColumn: "QuestionId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PointOfQuestions_Tests_TestId",
                table: "PointOfQuestions",
                column: "TestId",
                principalTable: "Tests",
                principalColumn: "TestId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PointOfQuestions_Questions_QuestionId",
                table: "PointOfQuestions");

            migrationBuilder.DropForeignKey(
                name: "FK_PointOfQuestions_Tests_TestId",
                table: "PointOfQuestions");

            migrationBuilder.DropTable(
                name: "AnswerOfStudents");

            migrationBuilder.DropTable(
                name: "StudentPoints");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PointOfQuestions",
                table: "PointOfQuestions");

            migrationBuilder.RenameTable(
                name: "PointOfQuestions",
                newName: "PointOfQuestion");

            migrationBuilder.RenameColumn(
                name: "PointOfTest",
                table: "Tests",
                newName: "Point");

            migrationBuilder.RenameIndex(
                name: "IX_PointOfQuestions_TestId",
                table: "PointOfQuestion",
                newName: "IX_PointOfQuestion_TestId");

            migrationBuilder.RenameIndex(
                name: "IX_PointOfQuestions_QuestionId",
                table: "PointOfQuestion",
                newName: "IX_PointOfQuestion_QuestionId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PointOfQuestion",
                table: "PointOfQuestion",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "StudentAnswers",
                columns: table => new
                {
                    StudentAnswerId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AnswerId = table.Column<int>(type: "integer", nullable: false),
                    ApplicationUserId = table.Column<string>(type: "text", nullable: false),
                    QuestionId = table.Column<int>(type: "integer", nullable: false),
                    TestId = table.Column<int>(type: "integer", nullable: false),
                    Completion = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Duration = table.Column<TimeOnly>(type: "time without time zone", nullable: true),
                    StudentAnswerPoint = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentAnswers", x => x.StudentAnswerId);
                    table.ForeignKey(
                        name: "FK_StudentAnswers_Answers_AnswerId",
                        column: x => x.AnswerId,
                        principalTable: "Answers",
                        principalColumn: "AnswerId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StudentAnswers_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StudentAnswers_Questions_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "Questions",
                        principalColumn: "QuestionId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StudentAnswers_Tests_TestId",
                        column: x => x.TestId,
                        principalTable: "Tests",
                        principalColumn: "TestId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StudentAnswers_AnswerId",
                table: "StudentAnswers",
                column: "AnswerId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentAnswers_ApplicationUserId",
                table: "StudentAnswers",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentAnswers_QuestionId",
                table: "StudentAnswers",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentAnswers_TestId",
                table: "StudentAnswers",
                column: "TestId");

            migrationBuilder.AddForeignKey(
                name: "FK_PointOfQuestion_Questions_QuestionId",
                table: "PointOfQuestion",
                column: "QuestionId",
                principalTable: "Questions",
                principalColumn: "QuestionId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PointOfQuestion_Tests_TestId",
                table: "PointOfQuestion",
                column: "TestId",
                principalTable: "Tests",
                principalColumn: "TestId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
