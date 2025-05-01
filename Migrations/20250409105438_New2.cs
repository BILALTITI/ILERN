using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Assigment2.Migrations
{
    /// <inheritdoc />
    public partial class New2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Trainees_Courses_CourseId",
                table: "Trainees");

            migrationBuilder.RenameColumn(
                name: "CourseId",
                table: "Trainees",
                newName: "courseId");

            migrationBuilder.RenameIndex(
                name: "IX_Trainees_CourseId",
                table: "Trainees",
                newName: "IX_Trainees_courseId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Courses",
                newName: "CourseId");

            migrationBuilder.AddForeignKey(
                name: "FK_Trainees_Courses_courseId",
                table: "Trainees",
                column: "courseId",
                principalTable: "Courses",
                principalColumn: "CourseId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Trainees_Courses_courseId",
                table: "Trainees");

            migrationBuilder.RenameColumn(
                name: "courseId",
                table: "Trainees",
                newName: "CourseId");

            migrationBuilder.RenameIndex(
                name: "IX_Trainees_courseId",
                table: "Trainees",
                newName: "IX_Trainees_CourseId");

            migrationBuilder.RenameColumn(
                name: "CourseId",
                table: "Courses",
                newName: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Trainees_Courses_CourseId",
                table: "Trainees",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "Id");
        }
    }
}
