using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EventApp.Migrations
{
    /// <inheritdoc />
    public partial class nullableplaceevent : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_events_places_placeId",
                table: "events");

            migrationBuilder.AddForeignKey(
                name: "FK_events_places_placeId",
                table: "events",
                column: "placeId",
                principalTable: "places",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_events_places_placeId",
                table: "events");

            migrationBuilder.AddForeignKey(
                name: "FK_events_places_placeId",
                table: "events",
                column: "placeId",
                principalTable: "places",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
