using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClassRoom.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class UpdateCourse : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Course");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Course");

            migrationBuilder.RenameColumn(
                name: "MinStudentToOpenClass",
                table: "Course",
                newName: "Level");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Course",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "Duration",
                table: "Course",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "Course",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "Course",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "CourseId",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_CourseId",
                table: "AspNetUsers",
                column: "CourseId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Course_CourseId",
                table: "AspNetUsers",
                column: "CourseId",
                principalTable: "Course",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Course_CourseId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_CourseId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Course");

            migrationBuilder.DropColumn(
                name: "Duration",
                table: "Course");

            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "Course");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "Course");

            migrationBuilder.DropColumn(
                name: "CourseId",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "Level",
                table: "Course",
                newName: "MinStudentToOpenClass");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Course",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Course",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");
        }
    }
}
