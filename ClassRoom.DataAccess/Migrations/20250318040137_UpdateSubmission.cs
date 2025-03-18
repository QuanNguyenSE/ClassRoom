﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClassRoom.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class UpdateSubmission : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Answer",
                table: "Submission",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Answer",
                table: "Submission");
        }
    }
}
