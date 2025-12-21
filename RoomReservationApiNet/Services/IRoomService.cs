using Microsoft.AspNetCore.Mvc;
using RoomReservationApiNet.DTOs;

namespace RoomReservationApiNet.Services
{
  public interface IRoomService
  {
    Task<IEnumerable<RoomDTO>> GetAllRooms();
    Task<ActionResult<RoomDTO>> GetRoom(int id);
    Task<IActionResult> PutRoom(int id, UpdateRoomDTO updateRoomDto);
    Task<ActionResult<RoomDTO>> PostRoom(CreateRoomDTO createRoomDto);
    Task<IActionResult> DeleteRoom(int id);
  }
}
