using Microsoft.AspNetCore.Mvc;
using RoomReservationApiNet.DTOs;

namespace RoomReservationApiNet.Services
{
  public interface IReservationStatusService
  {
    Task<IEnumerable<ReservationStatusDTO>> GetAllReservationStatuses();
    Task<ActionResult<ReservationStatusDTO>> GetReservationStatus(int id);
    Task<IActionResult> PutReservationStatus(int id, UpdateReservationStatusDTO updateReservationStatusDto);
    Task<ActionResult<ReservationStatusDTO>> PostReservationStatus(CreateReservationStatusDTO createReservationStatusDto);
    Task<IActionResult> DeleteReservationStatus(int id);
  }
}
