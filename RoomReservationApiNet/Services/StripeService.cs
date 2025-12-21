using Microsoft.Extensions.Options;
using RoomReservationApiNet.DTOs;
using RoomReservationApiNet.Models;
using RoomReservationApiNet.Repository;
using Stripe;
using Stripe.Checkout;

namespace RoomReservationApiNet.Services
{
    public class StripeService : IStripeService
    {
        private readonly StripeSettings _stripeSettings;
        private readonly IReservationRepository _reservationRepository;
        private readonly IUserRepository _userRepository;
        private readonly EmailService _emailService;
        private readonly ILogger<StripeService> _logger;

        public StripeService(
            IOptions<StripeSettings> stripeSettings,
            IReservationRepository reservationRepository,
            IUserRepository userRepository,
            EmailService emailService,
            ILogger<StripeService> logger)
        {
            _stripeSettings = stripeSettings.Value;
            _reservationRepository = reservationRepository;
            _userRepository = userRepository;
            _emailService = emailService;
            _logger = logger;

            StripeConfiguration.ApiKey = _stripeSettings.SecretKey;
        }

        public async Task<CheckoutSessionResponseDTO> CreateCheckoutSessionAsync(CreateCheckoutSessionDTO request)
        {
            var reservation = await _reservationRepository.GetReservationById(request.ReservationId);
            if (reservation == null)
            {
                throw new Exception("Reservation not found");
            }

            var options = new SessionCreateOptions
            {
                Locale = "en", // Force English language on Stripe Checkout page
                PaymentMethodTypes = new List<string> { "card" },
                LineItems = new List<SessionLineItemOptions>
                {
                    new SessionLineItemOptions
                    {
                        PriceData = new SessionLineItemPriceDataOptions
                        {
                            UnitAmount = (long)(request.Amount * 100), // Stripe uses cents
                            Currency = request.Currency,
                            ProductData = new SessionLineItemPriceDataProductDataOptions
                            {
                                Name = request.ProductName ?? $"Room Reservation #{request.ReservationId}",
                                Description = request.ProductDescription ?? $"Check-in: {reservation.CheckInDate:dd/MM/yyyy} - Check-out: {reservation.CheckOutDate:dd/MM/yyyy}"
                            }
                        },
                        Quantity = 1
                    }
                },
                Mode = "payment",
                SuccessUrl = $"{_stripeSettings.SuccessUrl}?session_id={{CHECKOUT_SESSION_ID}}&reservation_id={request.ReservationId}",
                CancelUrl = $"{_stripeSettings.CancelUrl}?reservation_id={request.ReservationId}",
                Metadata = new Dictionary<string, string>
                {
                    { "reservation_id", request.ReservationId.ToString() }
                }
            };

            var service = new SessionService();
            var session = await service.CreateAsync(options);

            _logger.LogInformation("Created Stripe checkout session {SessionId} for reservation {ReservationId}",
                session.Id, request.ReservationId);

            return new CheckoutSessionResponseDTO
            {
                SessionId = session.Id,
                SessionUrl = session.Url,
                PublishableKey = _stripeSettings.PublishableKey
            };
        }

        public async Task<bool> ConfirmPaymentAsync(string sessionId)
        {
            try
            {
                var service = new SessionService();
                var session = await service.GetAsync(sessionId);

                if (session.PaymentStatus == "paid")
                {
                    var reservationIdStr = session.Metadata.GetValueOrDefault("reservation_id");
                    if (int.TryParse(reservationIdStr, out int reservationId))
                    {
                        return await ConfirmReservationAndSendEmail(reservationId);
                    }
                }

                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error confirming payment for session {SessionId}", sessionId);
                return false;
            }
        }

        public async Task<bool> HandleWebhookAsync(string json, string stripeSignature)
        {
            try
            {
                var stripeEvent = EventUtility.ParseEvent(json);

                _logger.LogInformation("Received Stripe webhook event: {EventType}", stripeEvent.Type);

                if (stripeEvent.Type == Events.CheckoutSessionCompleted)
                {
                    var session = stripeEvent.Data.Object as Session;
                    if (session != null && session.PaymentStatus == "paid")
                    {
                        var reservationIdStr = session.Metadata.GetValueOrDefault("reservation_id");
                        if (int.TryParse(reservationIdStr, out int reservationId))
                        {
                            return await ConfirmReservationAndSendEmail(reservationId);
                        }
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error handling Stripe webhook");
                return false;
            }
        }

        private async Task<bool> ConfirmReservationAndSendEmail(int reservationId)
        {
            try
            {
                var reservation = await _reservationRepository.GetReservationById(reservationId);
                if (reservation == null)
                {
                    _logger.LogWarning("Reservation {ReservationId} not found for payment confirmation", reservationId);
                    return false;
                }

                // Update reservation status to confirmed (2)
                reservation.StatusId = 2;

                // Nullify navigation properties to avoid EF Core tracking conflicts
                reservation.User = null!;
                reservation.Room = null!;
                reservation.Status = null!;

                await _reservationRepository.UpdateReservation(reservation);

                // Get user for email
                var user = await _userRepository.GetUserEntityById(reservation.Email);
                if (user != null)
                {
                    string emailSubject = "Booking Confirmation - Payment Received";
                    string emailBody = $@"
                    <h1>Booking Confirmation</h1>
                    <p>Dear {user.FullName},</p>
                    <p>Your payment has been received and your booking is now confirmed.</p>
                    <p><strong>Reservation Details:</strong></p>
                    <ul>
                        <li>Reservation ID: {reservation.ReservationId}</li>
                        <li>Entry date: {reservation.CheckInDate:dd/MM/yyyy}</li>
                        <li>Departure date: {reservation.CheckOutDate:dd/MM/yyyy}</li>
                        <li>Number of nights: {reservation.NumberOfNights}</li>
                        <li>Number of guests: {reservation.NumberOfGuests}</li>
                    </ul>
                    <p>Thank you for choosing our hotel. We look forward to your stay!</p>
                    ";

                    await _emailService.SendEmailAsync(user.Email, emailSubject, emailBody, "confirmation", reservation.ReservationId);
                    _logger.LogInformation("Payment confirmed and email sent for reservation {ReservationId}", reservationId);
                }

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error confirming reservation {ReservationId} after payment", reservationId);
                return false;
            }
        }
    }
}
