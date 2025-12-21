using Microsoft.EntityFrameworkCore;
using RoomReservationApiNet.Data;
using RoomReservationApiNet.Models;

namespace RoomReservationApiNet.Repository
{
  public class ReservationStatusRepository : IReservationStatusRepository
  {
    private readonly AppDbContext _context;

    public ReservationStatusRepository(AppDbContext context)
    {
      _context = context;
    }

    public async Task<IEnumerable<ReservationStatus>> GetAllReservationStatuses()
    {
      return await _context.ReservationStatuses.ToListAsync();
    }

    public async Task<ReservationStatus?> GetReservationStatusById(int id)
    {
      return await _context.ReservationStatuses.FindAsync(id);
    }

    public async Task AddReservationStatus(ReservationStatus reservationStatus)
    {
      _context.ReservationStatuses.Add(reservationStatus);
      await _context.SaveChangesAsync();
    }

    public async Task UpdateReservationStatus(ReservationStatus reservationStatus)
    {
      _context.Entry(reservationStatus).State = EntityState.Modified;
      await _context.SaveChangesAsync();
    }

    public async Task DeleteReservationStatus(int id)
    {
      var reservationStatus = await _context.ReservationStatuses.FindAsync(id);
      if (reservationStatus != null)
      {
        _context.ReservationStatuses.Remove(reservationStatus);
        await _context.SaveChangesAsync();
      }
    }

    public async Task<bool> ReservationStatusExists(int id)
    {
      return await _context.ReservationStatuses.AnyAsync(e => e.StatusId == id);
    }
  }
}
