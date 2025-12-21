using System.ComponentModel.DataAnnotations;

namespace RoomReservationApiNet.DTOs
{
    public class CreateUserDTO
    {
        [Required]
        [EmailAddress]
        [StringLength(255)]
        public required string Email { get; set; }

        [Required]
        [StringLength(255, MinimumLength = 8)]
        public required string Password { get; set; }

        [StringLength(200)]
        public string? FullName { get; set; }

        [StringLength(20)]
        public string? PhoneNumber { get; set; }
    }
}
