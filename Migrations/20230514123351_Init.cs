using Microsoft.EntityFrameworkCore.Migrations;
using System;

#nullable disable

namespace EventApp.Migrations
{
	/// <inheritdoc />
	public partial class Init : Migration
	{
		/// <inheritdoc />
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.CreateTable(
				name: "events",
				columns: table => new
				{
					id = table.Column<Guid>(type: "uuid", nullable: false),
					name = table.Column<string>(type: "text", nullable: false),
					description = table.Column<string>(type: "text", nullable: false),
					image = table.Column<string>(type: "text", nullable: true),
					start = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
					end = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
					placeId = table.Column<Guid>(type: "uuid", nullable: false),
					favorit = table.Column<bool>(type: "boolean", nullable: false),
					type = table.Column<string>(type: "text", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_events", x => x.id);
				});
		}

		/// <inheritdoc />
		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropTable(
				name: "events");
		}
	}
}
