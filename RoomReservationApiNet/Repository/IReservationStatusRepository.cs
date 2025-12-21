using RoomReservationApiNet.Models;

namespace RoomReservationApiNet.Repository
{
  public interface IReservationStatusRepository
  {
    Task<IEnumerable<ReservationStatus>> GetAllReservationStatuses();
    Task<ReservationStatus?> GetReservationStatusById(int id);
    Task AddReservationStatus(ReservationStatus reservationStatus);
    Task UpdateReservationStatus(ReservationStatus reservationStatus);
    Task DeleteReservationStatus(int id);
    Task<bool> ReservationStatusExists(int id);
  }
}
