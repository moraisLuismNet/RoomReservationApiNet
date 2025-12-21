using System.ComponentModel.DataAnnotations;

namespace RoomReservationApiNet.DTOs
{
    public class CreateCheckoutSessionDTO
    {
        [Required]
        public int ReservationId { get; set; }
        
        [Required]
        public decimal Amount { get; set; }
        
        [Required]
        public string Currency { get; set; } = "eur";
        
        public string? ProductName { get; set; }
        
        public string? ProductDescription { get; set; }
    }
}
