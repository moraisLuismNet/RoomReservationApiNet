using Microsoft.AspNetCore.Mvc;
using RoomReservationApiNet.DTOs;
using RoomReservationApiNet.Models;
using RoomReservationApiNet.Repository;

namespace RoomReservationApiNet.Services
{
  public class UserService : IUserService
  {
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
      _userRepository = userRepository;
    }

    public async Task<IEnumerable<UserDTO>> GetAllUsers()
    {
      return await _userRepository.GetAllUsers();
    }

    public async Task<ActionResult<UserDTO>> GetUser(string email)
    {
      var userDto = await _userRepository.GetUserById(email);
      if (userDto == null)
      {
        return new NotFoundResult();
      }
      return userDto;
    }

    public async Task<IActionResult> PutUser(string email, UpdateUserDTO updateUserDto)
    {
      var user = await _userRepository.GetUserEntityById(email);
      if (user == null)
      {
        return new NotFoundResult();
      }

      var userModel = new User
      {
        Email = user.Email,
        PasswordHash = user.PasswordHash,
        FullName = updateUserDto.FullName ?? user.FullName ?? string.Empty,
        PhoneNumber = updateUserDto.PhoneNumber ?? user.PhoneNumber ?? string.Empty,
        IsActive = updateUserDto.IsActive ?? user.IsActive,
        UpdatedAt = DateTime.UtcNow
      };

      await _userRepository.UpdateUser(userModel);
      return new NoContentResult();
    }

    public async Task<ActionResult<UserDTO>> PostUser(CreateUserDTO createUserDto)
    {
      var user = new User
      {
        Email = createUserDto.Email,
        PasswordHash = BCrypt.Net.BCrypt.HashPassword(createUserDto.Password),
        FullName = createUserDto.FullName ?? string.Empty,
        PhoneNumber = createUserDto.PhoneNumber ?? string.Empty,
        CreatedAt = DateTime.UtcNow,
        UpdatedAt = DateTime.UtcNow,
        IsActive = true,
        Role = "client"
      };

      await _userRepository.AddUser(user);

      var userDto = new UserDTO
      {
        Email = user.Email,
        FullName = user.FullName,
        PhoneNumber = user.PhoneNumber,
        IsActive = user.IsActive,
        CreatedAt = user.CreatedAt,
        LastLogin = user.LastLogin,
        Role = user.Role
      };

      return userDto;
    }

    public async Task<IActionResult> DeleteUser(string email)
    {
      var user = await _userRepository.GetUserById(email);
      if (user == null)
      {
        return new NotFoundResult();
      }

      await _userRepository.DeleteUser(email);
      return new NoContentResult();
    }
  }
}
