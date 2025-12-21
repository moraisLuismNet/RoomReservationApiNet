using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using RoomReservationApiNet.Models;

namespace RoomReservationApiNet.Helpers
{
  public static class AuthHelpers
  {
    // Helper to validate a user's password
    public static bool ValidateUserPassword(string password, string passwordHash)
    {
      if (string.IsNullOrEmpty(password) || string.IsNullOrEmpty(passwordHash))
        return false;

      return BCrypt.Net.BCrypt.Verify(password, passwordHash);
    }

    // Helper to generate a JWT token for a user
    public static string GenerateJwtToken(User user, IConfiguration configuration)
    {
      if (user == null)
        throw new ArgumentNullException(nameof(user));

      var secret = configuration["Jwt:Secret"];
      if (string.IsNullOrEmpty(secret))
        throw new ArgumentNullException("Jwt:Secret", "The JWT secret key is not configured.");

      var tokenHandler = new JwtSecurityTokenHandler();
      var key = Encoding.ASCII.GetBytes(secret);

      // Ensure that the role is not null or empty
      var role = !string.IsNullOrEmpty(user.Role) ? user.Role : "client";

      var tokenDescriptor = new SecurityTokenDescriptor
      {
        Subject = new ClaimsIdentity(new[]
          {
                    new Claim(ClaimTypes.Name, user.Email),
                    new Claim(ClaimTypes.NameIdentifier, user.Email),
                    new Claim(ClaimTypes.Email, user.Email ?? string.Empty),
                    new Claim(ClaimTypes.Role, role)
                }),
        Expires = DateTime.UtcNow.AddHours(Convert.ToDouble(configuration["Jwt:ExpirationHours"])),
        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
      };

      var token = tokenHandler.CreateToken(tokenDescriptor);
      return tokenHandler.WriteToken(token);
    }

    // Helper to get default role if none is provided
    public static string GetDefaultRole(string role)
    {
      return !string.IsNullOrEmpty(role) ? role : "client";
    }

    // Helper to hash a password
    public static string HashPassword(string password)
    {
      if (string.IsNullOrEmpty(password))
        throw new ArgumentNullException(nameof(password));

      return BCrypt.Net.BCrypt.HashPassword(password);
    }
  }
}
