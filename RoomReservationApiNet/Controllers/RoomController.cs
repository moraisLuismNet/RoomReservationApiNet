using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RoomReservationApiNet.DTOs;
using RoomReservationApiNet.Services;

namespace RoomReservationApiNet.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class RoomController : ControllerBase
  {
    private readonly IRoomService _roomService;

    public RoomController(IRoomService roomService)
    {
      _roomService = roomService;
    }

    // GET: api/Room
    [HttpGet]
    public async Task<ActionResult<IEnumerable<RoomDTO>>> GetRooms()
    {
      return Ok(await _roomService.GetAllRooms());
    }

    // GET: api/Room/5
    [HttpGet("{id}")]
    public async Task<ActionResult<RoomDTO>> GetRoom(int id)
    {
      return await _roomService.GetRoom(id);
    }

    // PUT: api/Room/5
    [HttpPut("{id}")]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> PutRoom(int id, UpdateRoomDTO updateRoomDto)
    {
      return await _roomService.PutRoom(id, updateRoomDto);
    }

    // POST: api/Room
    [HttpPost]
    [Authorize(Roles = "admin")]
    public async Task<ActionResult<RoomDTO>> PostRoom(CreateRoomDTO createRoomDto)
    {
      return await _roomService.PostRoom(createRoomDto);
    }

    // DELETE: api/Room/5
    [HttpDelete("{id}")]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> DeleteRoom(int id)
    {
      return await _roomService.DeleteRoom(id);
    }
  }
}
