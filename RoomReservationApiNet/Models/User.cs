using System.ComponentModel.DataAnnotations;

namespace RoomReservationApiNet.Models
{
    public class User
    {
        [Key]
        [Required]
        [EmailAddress]
        [StringLength(255)]
        public required string Email { get; set; }

        [Required]
        [StringLength(255)]
        public required string PasswordHash { get; set; }

        [StringLength(200)]
        public string? FullName { get; set; }

        [StringLength(20)]
        public string PhoneNumber { get; set; } = string.Empty;

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? LastLogin { get; set; }

        [Required]
        [StringLength(50)]
        public string Role { get; set; } = "client";
    }
}
