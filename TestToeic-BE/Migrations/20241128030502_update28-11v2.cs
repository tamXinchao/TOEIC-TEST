using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TestToeic.Migrations
{
    /// <inheritdoc />
    public partial class update2811v2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StudentOfClasses_AspNetUsers_ApplicationUserId",
                table: "StudentOfClasses");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentOfClasses_Classes_ClassId",
                table: "StudentOfClasses");

            migrationBuilder.DropPrimaryKey(
                name: "PK_StudentOfClasses",
                table: "StudentOfClasses");

            migrationBuilder.RenameTable(
                name: "StudentOfClasses",
                newName: "MemberOfClasses");

            migrationBuilder.RenameIndex(
                name: "IX_StudentOfClasses_ClassId",
                table: "MemberOfClasses",
                newName: "IX_MemberOfClasses_ClassId");

            migrationBuilder.RenameIndex(
                name: "IX_StudentOfClasses_ApplicationUserId",
                table: "MemberOfClasses",
                newName: "IX_MemberOfClasses_ApplicationUserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MemberOfClasses",
                table: "MemberOfClasses",
                column: "MemberOfClassId");

            migrationBuilder.AddForeignKey(
                name: "FK_MemberOfClasses_AspNetUsers_ApplicationUserId",
                table: "MemberOfClasses",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MemberOfClasses_Classes_ClassId",
                table: "MemberOfClasses",
                column: "ClassId",
                principalTable: "Classes",
                principalColumn: "ClassId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MemberOfClasses_AspNetUsers_ApplicationUserId",
                table: "MemberOfClasses");

            migrationBuilder.DropForeignKey(
                name: "FK_MemberOfClasses_Classes_ClassId",
                table: "MemberOfClasses");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MemberOfClasses",
                table: "MemberOfClasses");

            migrationBuilder.RenameTable(
                name: "MemberOfClasses",
                newName: "StudentOfClasses");

            migrationBuilder.RenameIndex(
                name: "IX_MemberOfClasses_ClassId",
                table: "StudentOfClasses",
                newName: "IX_StudentOfClasses_ClassId");

            migrationBuilder.RenameIndex(
                name: "IX_MemberOfClasses_ApplicationUserId",
                table: "StudentOfClasses",
                newName: "IX_StudentOfClasses_ApplicationUserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_StudentOfClasses",
                table: "StudentOfClasses",
                column: "MemberOfClassId");

            migrationBuilder.AddForeignKey(
                name: "FK_StudentOfClasses_AspNetUsers_ApplicationUserId",
                table: "StudentOfClasses",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StudentOfClasses_Classes_ClassId",
                table: "StudentOfClasses",
                column: "ClassId",
                principalTable: "Classes",
                principalColumn: "ClassId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
