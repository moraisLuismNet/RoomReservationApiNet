using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RoomReservationApiNet.Migrations
{
    /// <inheritdoc />
    public partial class AddEmailQueueReservationRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EmailQueues_Reservations_ReservationId",
                table: "EmailQueues");

            migrationBuilder.AddForeignKey(
                name: "FK_EmailQueues_Reservations_ReservationId",
                table: "EmailQueues",
                column: "ReservationId",
                principalTable: "Reservations",
                principalColumn: "ReservationId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EmailQueues_Reservations_ReservationId",
                table: "EmailQueues");

            migrationBuilder.AddForeignKey(
                name: "FK_EmailQueues_Reservations_ReservationId",
                table: "EmailQueues",
                column: "ReservationId",
                principalTable: "Reservations",
                principalColumn: "ReservationId");
        }
    }
}
