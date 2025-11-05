using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace dotnet.Migrations
{
    /// <inheritdoc />
    public partial class Update : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "course_id",
                table: "courses",
                newName: "code");

            migrationBuilder.RenameIndex(
                name: "ix_courses_course_id",
                table: "courses",
                newName: "ix_courses_code");

            migrationBuilder.AlterColumn<string>(
                name: "student_id",
                table: "students",
                type: "varchar(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AddColumn<short>(
                name: "gender",
                table: "students",
                type: "smallint",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "major_code",
                table: "students",
                type: "varchar(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "name",
                table: "students",
                type: "varchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "year",
                table: "students",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "majors",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    code = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    deleted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_majors", x => x.id);
                    table.UniqueConstraint("ak_majors_code", x => x.code);
                });

            migrationBuilder.InsertData(
                table: "majors",
                columns: new[] { "id", "code", "created_at", "deleted_at", "is_active", "name", "updated_at" },
                values: new object[,]
                {
                    { 1, "CNPM", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, true, "Công nghệ phần mềm", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 2, "HTTT", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, true, "Hệ thống thông tin", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 3, "MMT", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, true, "Mạng máy tính", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) }
                });

            migrationBuilder.CreateIndex(
                name: "ix_students_major_code",
                table: "students",
                column: "major_code");

            migrationBuilder.CreateIndex(
                name: "ix_majors_code",
                table: "majors",
                column: "code",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "fk_students_majors_major_code",
                table: "students",
                column: "major_code",
                principalTable: "majors",
                principalColumn: "code");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_students_majors_major_code",
                table: "students");

            migrationBuilder.DropTable(
                name: "majors");

            migrationBuilder.DropIndex(
                name: "ix_students_major_code",
                table: "students");

            migrationBuilder.DropColumn(
                name: "gender",
                table: "students");

            migrationBuilder.DropColumn(
                name: "major_code",
                table: "students");

            migrationBuilder.DropColumn(
                name: "name",
                table: "students");

            migrationBuilder.DropColumn(
                name: "year",
                table: "students");

            migrationBuilder.RenameColumn(
                name: "code",
                table: "courses",
                newName: "course_id");

            migrationBuilder.RenameIndex(
                name: "ix_courses_code",
                table: "courses",
                newName: "ix_courses_course_id");

            migrationBuilder.AlterColumn<string>(
                name: "student_id",
                table: "students",
                type: "varchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "varchar(50)",
                oldMaxLength: 50,
                oldNullable: true);
        }
    }
}
