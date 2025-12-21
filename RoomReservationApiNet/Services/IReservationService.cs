using Microsoft.AspNetCore.Mvc;
using RoomReservationApiNet.DTOs;
using RoomReservationApiNet.Models;

namespace RoomReservationApiNet.Services
{
  public interface IReservationService
  {
    Task<IEnumerable<ReservationDTO>> GetAllReservations();
    Task<IEnumerable<ReservationDTO>> GetReservationsByEmail(string email);
    Task<ActionResult<ReservationDTO>> GetReservation(int id);
    Task<IActionResult> PutReservation(int id, Reservation reservation);
    Task<ActionResult<ReservationDTO>> PostReservation(CreateReservationDTO createReservationDto);
    Task<IActionResult> DeleteReservation(int id);
    Task<IEnumerable<ReservationDTO>> GetReservationsByRoomId(int roomId);
  }
}
