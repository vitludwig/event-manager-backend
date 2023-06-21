using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EventApp.Migrations
{
    /// <inheritdoc />
    public partial class eventplacerelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_events_placeId",
                table: "events",
                column: "placeId");

            migrationBuilder.AddForeignKey(
                name: "FK_events_places_placeId",
                table: "events",
                column: "placeId",
                principalTable: "places",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_events_places_placeId",
                table: "events");

            migrationBuilder.DropIndex(
                name: "IX_events_placeId",
                table: "events");
        }
    }
}
