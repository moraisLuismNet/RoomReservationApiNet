using Microsoft.AspNetCore.Mvc;
using RoomReservationApiNet.DTOs;

namespace RoomReservationApiNet.Services
{
  public interface IUserService
  {
    Task<IEnumerable<UserDTO>> GetAllUsers();
    Task<ActionResult<UserDTO>> GetUser(string email);
    Task<IActionResult> PutUser(string email, UpdateUserDTO updateUserDto);
    Task<ActionResult<UserDTO>> PostUser(CreateUserDTO createUserDto);
    Task<IActionResult> DeleteUser(string email);
  }
}
