using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RoomReservationApiNet.DTOs;
using RoomReservationApiNet.Models;
using RoomReservationApiNet.Services;

namespace RoomReservationApiNet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ReservationsController : ControllerBase
    {
        private readonly IReservationService _reservationService;

        public ReservationsController(IReservationService reservationService)
        {
            _reservationService = reservationService;
        }

        // GET: api/Reservations
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ReservationDTO>>> GetReservations()
        {
            if (!User.IsInRole("admin"))
            {
                return Forbid();
            }

            return Ok(await _reservationService.GetAllReservations());
        }

        // GET: api/Reservations/email
        [HttpGet("{email}")]
        public async Task<ActionResult<IEnumerable<ReservationDTO>>> GetReservation(string email)
        {
            if (!User.IsInRole("admin") && (User.Identity == null || User.Identity.Name != email))
            {
                return Forbid();
            }

            var reservations = await _reservationService.GetReservationsByEmail(email);
            if (reservations == null || !reservations.Any())
            {
                return NotFound();
            }

            return Ok(reservations);
        }

        // GET: api/Reservations/room/5
        [HttpGet("room/{roomId}")]
        [AllowAnonymous] // Allow viewing availability without login? Or maybe Authorize? User request says "When booking", so user is likely logged in. But safer to AllowAnonymous if we want public availability. I'll stick to Authorize for now as per controller attribute, or override. Let's make it public for now so anyone can see availability.
        public async Task<ActionResult<IEnumerable<ReservationDTO>>> GetReservationsByRoom(int roomId)
        {
             var reservations = await _reservationService.GetReservationsByRoomId(roomId);
             return Ok(reservations);
        }

        // PUT: api/Reservations/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutReservation(int id, Reservation reservation)
        {
            return await _reservationService.PutReservation(id, reservation);
        }

        // POST: api/Reservations
        [HttpPost]
        public async Task<ActionResult<ReservationDTO>> PostReservation(CreateReservationDTO createReservationDto)
        {
            return await _reservationService.PostReservation(createReservationDto);
        }

        // DELETE: api/Reservations/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReservation(int id)
        {
            return await _reservationService.DeleteReservation(id);
        }
    }
}
