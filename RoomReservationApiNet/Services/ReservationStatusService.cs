using Microsoft.AspNetCore.Mvc;
using RoomReservationApiNet.DTOs;
using RoomReservationApiNet.Models;
using RoomReservationApiNet.Repository;

namespace RoomReservationApiNet.Services
{
  public class ReservationStatusService : IReservationStatusService
  {
    private readonly IReservationStatusRepository _reservationStatusRepository;

    public ReservationStatusService(IReservationStatusRepository reservationStatusRepository)
    {
      _reservationStatusRepository = reservationStatusRepository;
    }

    public async Task<IEnumerable<ReservationStatusDTO>> GetAllReservationStatuses()
    {
      var reservationStatuses = await _reservationStatusRepository.GetAllReservationStatuses();
      var reservationStatusDtos = reservationStatuses.Select(rs => new ReservationStatusDTO
      {
        StatusId = rs.StatusId,
        Name = rs.Name
      }).ToList();

      return reservationStatusDtos;
    }

    public async Task<ActionResult<ReservationStatusDTO>> GetReservationStatus(int id)
    {
      var reservationStatus = await _reservationStatusRepository.GetReservationStatusById(id);
      if (reservationStatus == null)
      {
        return new NotFoundResult();
      }

      var reservationStatusDto = new ReservationStatusDTO
      {
        StatusId = reservationStatus.StatusId,
        Name = reservationStatus.Name
      };

      return reservationStatusDto;
    }

    public async Task<IActionResult> PutReservationStatus(int id, UpdateReservationStatusDTO updateReservationStatusDto)
    {
      var existingReservationStatus = await _reservationStatusRepository.GetReservationStatusById(id);
      if (existingReservationStatus == null)
      {
        return new NotFoundResult();
      }

      existingReservationStatus.Name = updateReservationStatusDto.Name;

      await _reservationStatusRepository.UpdateReservationStatus(existingReservationStatus);
      return new NoContentResult();
    }

    public async Task<ActionResult<ReservationStatusDTO>> PostReservationStatus(CreateReservationStatusDTO createReservationStatusDto)
    {
      var reservationStatus = new ReservationStatus
      {
        Name = createReservationStatusDto.Name
      };

      await _reservationStatusRepository.AddReservationStatus(reservationStatus);

      var reservationStatusDto = new ReservationStatusDTO
      {
        StatusId = reservationStatus.StatusId,
        Name = reservationStatus.Name
      };

      return reservationStatusDto;
    }

    public async Task<IActionResult> DeleteReservationStatus(int id)
    {
      var reservationStatus = await _reservationStatusRepository.GetReservationStatusById(id);
      if (reservationStatus == null)
      {
        return new NotFoundResult();
      }

      await _reservationStatusRepository.DeleteReservationStatus(id);
      return new NoContentResult();
    }
  }
}
