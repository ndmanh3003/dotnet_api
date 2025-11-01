using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace dotnet.Migrations
{
    /// <inheritdoc />
    public partial class InitDatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "code",
                table: "courses",
                newName: "course_id");

            migrationBuilder.RenameIndex(
                name: "ix_courses_code",
                table: "courses",
                newName: "ix_courses_course_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "course_id",
                table: "courses",
                newName: "code");

            migrationBuilder.RenameIndex(
                name: "ix_courses_course_id",
                table: "courses",
                newName: "ix_courses_code");
        }
    }
}
