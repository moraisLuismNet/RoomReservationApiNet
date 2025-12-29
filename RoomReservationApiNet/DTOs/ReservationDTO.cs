namespace RoomReservationApiNet.DTOs
{
    public class ReservationDTO
    {
        public int ReservationId { get; set; }
        public int StatusId { get; set; }
        public UserDTO? User { get; set; }
        public RoomDTO? Room { get; set; }
        public int RoomId { get; set; }
        public DateTime ReservationDate { get; set; }
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        public int NumberOfNights { get; set; }
        public int NumberOfGuests { get; set; }
        public DateTime? CancellationDate { get; set; }
        public string? CancellationReason { get; set; }
    }
}
