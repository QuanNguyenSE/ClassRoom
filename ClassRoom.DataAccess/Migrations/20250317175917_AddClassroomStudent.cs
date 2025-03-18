using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClassRoom.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddClassroomStudent : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Classroom_ClassroomId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_ClassroomId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "ClassroomId",
                table: "AspNetUsers");

            migrationBuilder.CreateTable(
                name: "ClassroomStudent",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClassroomId = table.Column<int>(type: "int", nullable: false),
                    StudentId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClassroomStudent", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClassroomStudent_AspNetUsers_StudentId",
                        column: x => x.StudentId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ClassroomStudent_Classroom_ClassroomId",
                        column: x => x.ClassroomId,
                        principalTable: "Classroom",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ClassroomStudent_ClassroomId",
                table: "ClassroomStudent",
                column: "ClassroomId");

            migrationBuilder.CreateIndex(
                name: "IX_ClassroomStudent_StudentId",
                table: "ClassroomStudent",
                column: "StudentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ClassroomStudent");

            migrationBuilder.AddColumn<int>(
                name: "ClassroomId",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_ClassroomId",
                table: "AspNetUsers",
                column: "ClassroomId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Classroom_ClassroomId",
                table: "AspNetUsers",
                column: "ClassroomId",
                principalTable: "Classroom",
                principalColumn: "Id");
        }
    }
}
