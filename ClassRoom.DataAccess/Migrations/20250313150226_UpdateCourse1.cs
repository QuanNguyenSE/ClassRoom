using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClassRoom.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class UpdateCourse1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MinStudentToOpenClass",
                table: "Course",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MinStudentToOpenClass",
                table: "Course");
        }
    }
}
