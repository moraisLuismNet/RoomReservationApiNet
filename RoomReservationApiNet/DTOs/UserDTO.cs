namespace RoomReservationApiNet.DTOs
{
    public class UserDTO
    {
        public string? Email { get; set; }
        public string? FullName { get; set; }
        public string? PhoneNumber { get; set; }
        public bool IsEmailVerified { get; set; }
        public bool IsActive { get; set; }
        public System.DateTime CreatedAt { get; set; }
        public System.DateTime? LastLogin { get; set; }
        public string? Role { get; set; }
    }
}