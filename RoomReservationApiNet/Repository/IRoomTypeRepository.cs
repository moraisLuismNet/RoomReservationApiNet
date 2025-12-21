using RoomReservationApiNet.Models;

namespace RoomReservationApiNet.Repository
{
  public interface IRoomTypeRepository
  {
    Task<IEnumerable<RoomType>> GetAllRoomTypes();
    Task<RoomType?> GetRoomTypeById(int id);
    Task AddRoomType(RoomType roomType);
    Task UpdateRoomType(RoomType roomType);
    Task DeleteRoomType(int id);
    Task<bool> RoomTypeExists(int id);
  }
}
