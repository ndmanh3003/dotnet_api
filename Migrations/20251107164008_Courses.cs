using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace dotnet.Migrations
{
    /// <inheritdoc />
    public partial class Courses : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "courses",
                columns: new[] { "id", "code", "created_at", "credits", "deleted_at", "exercise_hours", "name", "practice_hours", "theory_hours", "type", "updated_at" },
                values: new object[,]
                {
                    { 1, "CS101", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), (short)3, null, (short)15, "Nhập môn lập trình", (short)15, (short)30, (short)1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 2, "CS201", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), (short)3, null, (short)15, "Cấu trúc dữ liệu và giải thuật", (short)15, (short)30, (short)1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 3, "CS301", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), (short)4, null, (short)15, "Kỹ năng mềm", (short)30, (short)30, (short)2, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "courses",
                keyColumn: "id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "courses",
                keyColumn: "id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "courses",
                keyColumn: "id",
                keyValue: 3);
        }
    }
}
