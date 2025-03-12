using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClassRoom.DataAccess.Migrations
{
	/// <inheritdoc />
	public partial class AddColumnToUser : Migration
	{
		/// <inheritdoc />
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.AddColumn<string>(
				name: "Discriminator",
				table: "AspNetUsers",
				type: "nvarchar(21)",
				maxLength: 21,
				nullable: false,
				defaultValue: "Student");

			migrationBuilder.AddColumn<DateTime>(
				name: "EnrollmentDate",
				table: "AspNetUsers",
				type: "datetime2",
				nullable: true);

			migrationBuilder.AddColumn<string>(
				name: "FirstName",
				table: "AspNetUsers",
				type: "nvarchar(50)",
				maxLength: 50,
				nullable: true);

			migrationBuilder.AddColumn<DateTime>(
				name: "HireDate",
				table: "AspNetUsers",
				type: "datetime2",
				nullable: true);

			migrationBuilder.AddColumn<string>(
				name: "LastName",
				table: "AspNetUsers",
				type: "nvarchar(50)",
				maxLength: 50,
				nullable: true);
		}

		/// <inheritdoc />
		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropColumn(
				name: "Discriminator",
				table: "AspNetUsers");

			migrationBuilder.DropColumn(
				name: "EnrollmentDate",
				table: "AspNetUsers");

			migrationBuilder.DropColumn(
				name: "FirstName",
				table: "AspNetUsers");

			migrationBuilder.DropColumn(
				name: "HireDate",
				table: "AspNetUsers");

			migrationBuilder.DropColumn(
				name: "LastName",
				table: "AspNetUsers");
		}
	}
}
