using RoomReservationApiNet.Models;

namespace RoomReservationApiNet.Repository
{
  public interface IRoomRepository
  {
    Task<IEnumerable<Room>> GetAllRooms();
    Task<Room?> GetRoomById(int id);
    Task AddRoom(Room room);
    Task UpdateRoom(Room room);
    Task DeleteRoom(int id);
    Task<bool> RoomExists(int id);
  }
}
