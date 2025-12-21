using Microsoft.EntityFrameworkCore;
using RoomReservationApiNet.Models;
using RoomReservationApiNet.Data;
using RoomReservationApiNet.Helpers;

namespace RoomReservationApiNet.Services
{
    public class AuthService
    {
        private readonly IConfiguration _configuration;
        private readonly AppDbContext _context;

        public AuthService(IConfiguration configuration, AppDbContext context)
        {
            _configuration = configuration;
            _context = context;
        }

        public string GenerateJwtToken(User user)
        {
            return AuthHelpers.GenerateJwtToken(user, _configuration);
        }

        public async Task<bool> ValidateUser(string email, string password)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null)
                return false;

            // Verify the encrypted password using the helper
            return AuthHelpers.ValidateUserPassword(password, user.PasswordHash);
        }

        public async Task<User?> GetUserByEmail(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<User> RegisterUser(User user, string password)
        {
            // Validate that the user is not null
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            // Set default values for the user
            user.Role = AuthHelpers.GetDefaultRole(user.Role);
            user.PhoneNumber ??= string.Empty;
            user.FullName ??= string.Empty;
            user.PasswordHash = AuthHelpers.HashPassword(password);
            user.CreatedAt = DateTime.UtcNow;
            user.UpdatedAt = DateTime.UtcNow;
            user.IsActive = true;

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return user;
        }

        public Task<bool> InvalidateToken(string token)
        {
            // Since we're not storing tokens in the database anymore,
            // we can't invalidate individual tokens.
            // In a production environment, you might want to implement a token blacklist
            // or use refresh tokens with a shorter expiration time.
            return Task.FromResult(true);
        }
    }
}
