using Microsoft.AspNetCore.Mvc;
using RoomReservationApiNet.Models;
using RoomReservationApiNet.Services;

namespace RoomReservationApiNet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;
        private readonly IConfiguration _configuration;

        public AuthController(AuthService authService, IConfiguration configuration)
        {
            _authService = authService;
            _configuration = configuration;
        }

        [HttpPost("login")]
        public async Task<ActionResult<LoginResponseDTO>> Login([FromBody] LoginRequestDTO loginRequestDto)
        {
            var isValid = await _authService.ValidateUser(loginRequestDto.Email, loginRequestDto.Password);
            if (!isValid)
            {
                return Unauthorized("Invalid credentials");
            }

            var user = await _authService.GetUserByEmail(loginRequestDto.Email);
            if (user == null)
            {
                return Unauthorized("User not found");
            }

            var token = _authService.GenerateJwtToken(user);
            
            var expiresInHours = _configuration.GetValue<int>("Jwt:ExpirationHours");
            var expiresAt = DateTime.UtcNow.AddHours(expiresInHours);

            var response = new LoginResponseDTO
            {
                Token = token,
                Email = user.Email,
                Role = user.Role,
                ExpiresAt = expiresAt
            };

            return Ok(response);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDTO request)
        {
            var user = new User
            {
                Email = request.Email,
                FullName = request.FullName,
                PhoneNumber = request.PhoneNumber,
                PasswordHash = "default" // Temporary value, will be correctly set in RegisterUser
            };

            var registeredUser = await _authService.RegisterUser(user, request.Password);
            if (registeredUser == null)
            {
                return BadRequest("User registration failed");
            }
            return Ok(new { Message = "User registered successfully", registeredUser.Email });
        }
    }
}
