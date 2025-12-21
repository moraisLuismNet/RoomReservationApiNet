using RoomReservationApiNet.DTOs;
using RoomReservationApiNet.Models;

namespace RoomReservationApiNet.Repository
{
  public interface IUserRepository
  {
    Task<IEnumerable<UserDTO>> GetAllUsers();
    Task<UserDTO?> GetUserById(string email);
    Task<User?> GetUserByEmail(string email);
    Task<User?> GetUserEntityById(string email);
    Task AddUser(User user);
    Task UpdateUser(User user);
    Task DeleteUser(string email);
    Task<bool> UserExists(string email);
  }
}
