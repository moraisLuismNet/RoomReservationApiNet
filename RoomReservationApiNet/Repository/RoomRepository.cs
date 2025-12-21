using Microsoft.EntityFrameworkCore;
using RoomReservationApiNet.Data;
using RoomReservationApiNet.Models;

namespace RoomReservationApiNet.Repository
{
  public class RoomRepository : IRoomRepository
  {
    private readonly AppDbContext _context;

    public RoomRepository(AppDbContext context)
    {
      _context = context;
    }

    public async Task<IEnumerable<Room>> GetAllRooms()
    {
      return await _context.Rooms
          .Include(r => r.RoomType)
          .ToListAsync();
    }

    public async Task<Room?> GetRoomById(int id)
    {
      return await _context.Rooms
          .Include(r => r.RoomType)
          .FirstOrDefaultAsync(r => r.RoomId == id);
    }

    public async Task AddRoom(Room room)
    {
      _context.Rooms.Add(room);
      await _context.SaveChangesAsync();
    }

    public async Task UpdateRoom(Room room)
    {
      _context.Entry(room).State = EntityState.Modified;
      await _context.SaveChangesAsync();
    }

    public async Task DeleteRoom(int id)
    {
      var room = await _context.Rooms.FindAsync(id);
      if (room != null)
      {
        _context.Rooms.Remove(room);
        await _context.SaveChangesAsync();
      }
    }

    public async Task<bool> RoomExists(int id)
    {
      return await _context.Rooms.AnyAsync(e => e.RoomId == id);
    }
  }
}
