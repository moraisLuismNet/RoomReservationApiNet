using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RoomReservationApiNet.DTOs;
using RoomReservationApiNet.Services;

namespace RoomReservationApiNet.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class RoomTypeController : ControllerBase
  {
    private readonly IRoomTypeService _roomTypeService;

    public RoomTypeController(IRoomTypeService roomTypeService)
    {
      _roomTypeService = roomTypeService;
    }

    // GET: api/RoomType
    [HttpGet]
    public async Task<ActionResult<IEnumerable<RoomTypeDTO>>> GetRoomTypes()
    {
      return Ok(await _roomTypeService.GetAllRoomTypes());
    }

    // GET: api/RoomType/5
    [HttpGet("{id}")]
    public async Task<ActionResult<RoomTypeDTO>> GetRoomType(int id)
    {
      var roomType = await _roomTypeService.GetRoomType(id);
      if (roomType.Result == null)
      {
        return NotFound();
      }

      return roomType;
    }

    // PUT: api/RoomType/5
    [HttpPut("{id}")]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> PutRoomType(int id, UpdateRoomTypeDTO updateRoomTypeDto)
    {
      return await _roomTypeService.PutRoomType(id, updateRoomTypeDto);
    }

    // POST: api/RoomType
    [HttpPost]
    [Authorize(Roles = "admin")]
    public async Task<ActionResult<RoomTypeDTO>> PostRoomType(CreateRoomTypeDTO createRoomTypeDto)
    {
      return await _roomTypeService.PostRoomType(createRoomTypeDto);
    }

    // DELETE: api/RoomType/5
    [HttpDelete("{id}")]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> DeleteRoomType(int id)
    {
      return await _roomTypeService.DeleteRoomType(id);
    }
  }
}
