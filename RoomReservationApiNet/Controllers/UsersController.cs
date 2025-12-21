using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RoomReservationApiNet.DTOs;
using RoomReservationApiNet.Services;

namespace RoomReservationApiNet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "admin")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDTO>>> GetUsers()
        {
            return Ok(await _userService.GetAllUsers());
        }

        // GET: api/Users/5
        [HttpGet("{email}")]
        public async Task<ActionResult<UserDTO>> GetUser(string email)
        {
            return await _userService.GetUser(email);
        }

        // PUT: api/Users/5
        [HttpPut("{email}")]
        public async Task<IActionResult> PutUser(string email, UpdateUserDTO updateUserDto)
        {
            return await _userService.PutUser(email, updateUserDto);
        }

        // POST: api/Users
        [HttpPost]
        public async Task<ActionResult<UserDTO>> PostUser(CreateUserDTO createUserDto)
        {
            return await _userService.PostUser(createUserDto);
        }

        // DELETE: api/Users/5
        [HttpDelete("{email}")]
        public async Task<IActionResult> DeleteUser(string email)
        {
            return await _userService.DeleteUser(email);
        }
    }
}
