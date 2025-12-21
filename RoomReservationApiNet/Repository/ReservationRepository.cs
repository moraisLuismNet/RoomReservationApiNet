using Microsoft.EntityFrameworkCore;
using RoomReservationApiNet.Data;
using RoomReservationApiNet.DTOs;
using RoomReservationApiNet.Models;

namespace RoomReservationApiNet.Repository
{
  public class ReservationRepository : IReservationRepository
  {
    private readonly AppDbContext _context;

    public ReservationRepository(AppDbContext context)
    {
      _context = context;
    }

    public async Task<IEnumerable<ReservationDTO>> GetAllReservations()
    {
      var reservations = await _context.Reservations
          .Include(r => r.User)
          .Include(r => r.Room)
          .Include(r => r.Status)
          .ToListAsync();

      var reservationDtos = reservations.Select(r => new ReservationDTO
      {
        ReservationId = r.ReservationId,
        StatusId = r.StatusId,
        User = new UserDTO
        {
          Email = r.User.Email,
          FullName = r.User.FullName
        },
        RoomId = r.RoomId,
        ReservationDate = r.ReservationDate,
        CheckInDate = r.CheckInDate,
        CheckOutDate = r.CheckOutDate,
        NumberOfNights = r.NumberOfNights,
        NumberOfGuests = r.NumberOfGuests,
        CancellationDate = r.CancellationDate,
        CancellationReason = r.CancellationReason
      }).ToList();

      return reservationDtos;
    }

    public async Task<IEnumerable<ReservationDTO>> GetReservationsByEmail(string email)
    {
      var reservations = await _context.Reservations
          .Include(r => r.User)
          .Include(r => r.Room)
          .Include(r => r.Status)
          .Where(r => r.User.Email == email)
          .ToListAsync();

      var reservationDtos = reservations.Select(r => new ReservationDTO
      {
        ReservationId = r.ReservationId,
        StatusId = r.StatusId,
        User = new UserDTO
        {
          Email = r.User.Email,
          FullName = r.User.FullName
        },
        RoomId = r.RoomId,
        ReservationDate = r.ReservationDate,
        CheckInDate = r.CheckInDate,
        CheckOutDate = r.CheckOutDate,
        NumberOfNights = r.NumberOfNights,
        NumberOfGuests = r.NumberOfGuests,
        CancellationDate = r.CancellationDate,
        CancellationReason = r.CancellationReason
      }).ToList();

      return reservationDtos;
    }

    public async Task<Reservation?> GetReservationById(int id)
    {
      return await _context.Reservations.AsNoTracking().Include(r => r.User).FirstOrDefaultAsync(r => r.ReservationId == id);
    }

    public async Task AddReservation(Reservation reservation)
    {
      _context.Reservations.Add(reservation);
      await _context.SaveChangesAsync();
    }

    public async Task UpdateReservation(Reservation reservation)
    {
      _context.Entry(reservation).State = EntityState.Modified;
      await _context.SaveChangesAsync();
    }

    public async Task DeleteReservation(int id)
    {
      var reservation = await _context.Reservations.FindAsync(id);
      if (reservation != null)
      {
        _context.Reservations.Remove(reservation);
        await _context.SaveChangesAsync();
      }
    }

    public async Task<bool> ReservationExists(int id)
    {
      return await _context.Reservations.AnyAsync(e => e.ReservationId == id);
    }

    public async Task<bool> IsRoomAvailable(int roomId, System.DateTime checkIn, System.DateTime checkOut)
    {
      return !await _context.Reservations
          .AnyAsync(r => r.RoomId == roomId &&
                        r.StatusId != 5 && // 5 = cancelled
                        r.StatusId != 6 && // 6 = no-show
                        ((checkIn >= r.CheckInDate && checkIn < r.CheckOutDate) ||
                         (checkOut > r.CheckInDate && checkOut <= r.CheckOutDate) ||
                         (checkIn <= r.CheckInDate && checkOut >= r.CheckOutDate)));
    }

    public async Task<IEnumerable<ReservationDTO>> GetReservationsByRoomId(int roomId)
    {
        var reservations = await _context.Reservations
          .Include(r => r.User)
          .Include(r => r.Room)
          .Include(r => r.Status)
          .Where(r => r.RoomId == roomId && r.StatusId != 5 && r.StatusId != 6) // Exclude cancelled/no-show
          .ToListAsync();

        var reservationDtos = reservations.Select(r => new ReservationDTO
        {
            ReservationId = r.ReservationId,
            StatusId = r.StatusId,
            User = new UserDTO
            {
                Email = r.User.Email,
                FullName = r.User.FullName
            },
            RoomId = r.RoomId,
            ReservationDate = r.ReservationDate,
            CheckInDate = r.CheckInDate,
            CheckOutDate = r.CheckOutDate,
            NumberOfNights = r.NumberOfNights,
            NumberOfGuests = r.NumberOfGuests,
            CancellationDate = r.CancellationDate,
            CancellationReason = r.CancellationReason
        }).ToList();

        return reservationDtos;
    }
  }
}
