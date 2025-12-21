using Microsoft.EntityFrameworkCore;
using RoomReservationApiNet.Data;
using RoomReservationApiNet.Models;

namespace RoomReservationApiNet.Repository
{
  public class RoomTypeRepository : IRoomTypeRepository
  {
    private readonly AppDbContext _context;

    public RoomTypeRepository(AppDbContext context)
    {
      _context = context;
    }

    public async Task<IEnumerable<RoomType>> GetAllRoomTypes()
    {
      return await _context.RoomTypes.ToListAsync();
    }

    public async Task<RoomType?> GetRoomTypeById(int id)
    {
      return await _context.RoomTypes.FindAsync(id);
    }

    public async Task AddRoomType(RoomType roomType)
    {
      _context.RoomTypes.Add(roomType);
      await _context.SaveChangesAsync();
    }

    public async Task UpdateRoomType(RoomType roomType)
    {
      _context.Entry(roomType).State = EntityState.Modified;
      await _context.SaveChangesAsync();
    }

    public async Task DeleteRoomType(int id)
    {
      var roomType = await _context.RoomTypes.FindAsync(id);
      if (roomType != null)
      {
        _context.RoomTypes.Remove(roomType);
        await _context.SaveChangesAsync();
      }
    }

    public async Task<bool> RoomTypeExists(int id)
    {
      return await _context.RoomTypes.AnyAsync(e => e.RoomTypeId == id);
    }
  }
}
