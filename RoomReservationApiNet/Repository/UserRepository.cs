using Microsoft.EntityFrameworkCore;
using RoomReservationApiNet.Data;
using RoomReservationApiNet.DTOs;
using RoomReservationApiNet.Models;

namespace RoomReservationApiNet.Repository
{
  public class UserRepository : IUserRepository
  {
    private readonly AppDbContext _context;

    public UserRepository(AppDbContext context)
    {
      _context = context;
    }

    public async Task<IEnumerable<UserDTO>> GetAllUsers()
    {
      var users = await _context.Users.ToListAsync();
      var userDtos = users.Select(user => new UserDTO
      {
        Email = user.Email,
        FullName = user.FullName,
        PhoneNumber = user.PhoneNumber,
        IsActive = user.IsActive,
        CreatedAt = user.CreatedAt,
        LastLogin = user.LastLogin,
        Role = user.Role
      }).ToList();
      return userDtos;
    }

    public async Task<UserDTO?> GetUserById(string email)
    {
      var user = await _context.Users.FindAsync(email);
      if (user == null)
      {
        return null;
      }

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

    public async Task<User?> GetUserEntityById(string email)
    {
      return await _context.Users.FindAsync(email);
    }

    public async Task<User?> GetUserByEmail(string email)
    {
      return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task AddUser(User user)
    {
      _context.Users.Add(user);
      await _context.SaveChangesAsync();
    }

    public async Task UpdateUser(User user)
    {
      _context.Entry(user).State = EntityState.Modified;
      await _context.SaveChangesAsync();
    }

    public async Task DeleteUser(string email)
    {
      var user = await _context.Users.FindAsync(email);
      if (user != null)
      {
        _context.Users.Remove(user);
        await _context.SaveChangesAsync();
      }
    }

    public async Task<bool> UserExists(string email)
    {
      return await _context.Users.AnyAsync(e => e.Email == email);
    }
  }
}
