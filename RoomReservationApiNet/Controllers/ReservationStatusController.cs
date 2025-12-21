using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RoomReservationApiNet.DTOs;
using RoomReservationApiNet.Services;

namespace RoomReservationApiNet.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  [Authorize(Roles = "admin")]
  public class ReservationStatusController : ControllerBase
  {
    private readonly IReservationStatusService _reservationStatusService;

    public ReservationStatusController(IReservationStatusService reservationStatusService)
    {
      _reservationStatusService = reservationStatusService;
    }

    // GET: api/ReservationStatus
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ReservationStatusDTO>>> GetReservationStatuses()
    {
      return Ok(await _reservationStatusService.GetAllReservationStatuses());
    }

    // GET: api/ReservationStatus/5
    [HttpGet("{id}")]
    public async Task<ActionResult<ReservationStatusDTO>> GetReservationStatus(int id)
    {
      var reservationStatus = await _reservationStatusService.GetReservationStatus(id);
      if (reservationStatus.Result == null)
      {
        return NotFound();
      }

      return reservationStatus;
    }

    // PUT: api/ReservationStatus/5
    [HttpPut("{id}")]
    public async Task<IActionResult> PutReservationStatus(int id, UpdateReservationStatusDTO updateReservationStatusDto)
    {
      return await _reservationStatusService.PutReservationStatus(id, updateReservationStatusDto);
    }

    // POST: api/ReservationStatus
    [HttpPost]
    public async Task<ActionResult<ReservationStatusDTO>> PostReservationStatus(CreateReservationStatusDTO createReservationStatusDto)
    {
      return await _reservationStatusService.PostReservationStatus(createReservationStatusDto);
    }

    // DELETE: api/ReservationStatus/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteReservationStatus(int id)
    {
      return await _reservationStatusService.DeleteReservationStatus(id);
    }
  }
}
