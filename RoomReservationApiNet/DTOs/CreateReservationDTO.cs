using System.ComponentModel.DataAnnotations;

namespace RoomReservationApiNet.DTOs
{
    public class CreateReservationDTO
    {
        [Required]
        public int RoomId { get; set; }

        [Required]
        public DateTime CheckInDate { get; set; }

        [Required]
        public DateTime CheckOutDate { get; set; }

        [Required]
        public int NumberOfGuests { get; set; }

        public string? Email { get; set; }

        public DateTime ReservationDate { get; set; }

        public int NumberOfNights { get; set; }
    }
}
