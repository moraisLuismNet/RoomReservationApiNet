using RoomReservationApiNet.DTOs;

namespace RoomReservationApiNet.Services
{
  public interface IStripeService
  {
    Task<CheckoutSessionResponseDTO> CreateCheckoutSessionAsync(CreateCheckoutSessionDTO request);
    Task<bool> HandleWebhookAsync(string json, string stripeSignature);
    Task<bool> ConfirmPaymentAsync(string sessionId);
  }
}
