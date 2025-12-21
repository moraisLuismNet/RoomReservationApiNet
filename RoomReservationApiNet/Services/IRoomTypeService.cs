using Microsoft.AspNetCore.Mvc;
using RoomReservationApiNet.DTOs;

namespace RoomReservationApiNet.Services
{
  public interface IRoomTypeService
  {
    Task<IEnumerable<RoomTypeDTO>> GetAllRoomTypes();
    Task<ActionResult<RoomTypeDTO>> GetRoomType(int id);
    Task<IActionResult> PutRoomType(int id, UpdateRoomTypeDTO updateRoomTypeDto);
    Task<ActionResult<RoomTypeDTO>> PostRoomType(CreateRoomTypeDTO createRoomTypeDto);
    Task<IActionResult> DeleteRoomType(int id);
  }
}
