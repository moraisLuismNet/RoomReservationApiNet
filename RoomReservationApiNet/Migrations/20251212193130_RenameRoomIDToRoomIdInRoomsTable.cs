using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RoomReservationApiNet.Migrations
{
    /// <inheritdoc />
    public partial class RenameRoomIDToRoomIdInRoomsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "RoomID",
                table: "Rooms",
                newName: "RoomId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "RoomId",
                table: "Rooms",
                newName: "RoomID");
        }
    }
}
