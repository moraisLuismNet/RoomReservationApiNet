namespace RoomReservationApiNet.DTOs
{
    public class CheckoutSessionResponseDTO
    {
        public string SessionId { get; set; } = string.Empty;
        public string SessionUrl { get; set; } = string.Empty;
        public string PublishableKey { get; set; } = string.Empty;
    }
}
