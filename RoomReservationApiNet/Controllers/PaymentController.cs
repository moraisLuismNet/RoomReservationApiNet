using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RoomReservationApiNet.DTOs;
using RoomReservationApiNet.Services;

namespace RoomReservationApiNet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IStripeService _stripeService;
        private readonly ILogger<PaymentController> _logger;

        public PaymentController(IStripeService stripeService, ILogger<PaymentController> logger)
        {
            _stripeService = stripeService;
            _logger = logger;
        }

        /// <summary>
        /// Creates a Stripe checkout session for a reservation
        /// </summary>
        [HttpPost("create-checkout-session")]
        [Authorize]
        public async Task<ActionResult<CheckoutSessionResponseDTO>> CreateCheckoutSession([FromBody] CreateCheckoutSessionDTO request)
        {
            try
            {
                var response = await _stripeService.CreateCheckoutSessionAsync(request);
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating checkout session for reservation {ReservationId}", request.ReservationId);
                return BadRequest(new { error = ex.Message });
            }
        }

        /// <summary>
        /// Confirms payment after successful Stripe checkout
        /// </summary>
        [HttpPost("confirm")]
        public async Task<IActionResult> ConfirmPayment([FromBody] ConfirmPaymentDTO request)
        {
            try
            {
                var success = await _stripeService.ConfirmPaymentAsync(request.SessionId);
                if (success)
                {
                    return Ok(new { message = "Payment confirmed successfully" });
                }
                return BadRequest(new { error = "Payment confirmation failed" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error confirming payment for session {SessionId}", request.SessionId);
                return BadRequest(new { error = ex.Message });
            }
        }

        /// <summary>
        /// Stripe webhook endpoint for payment events
        /// </summary>
        [HttpPost("webhook")]
        public async Task<IActionResult> Webhook()
        {
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
            var stripeSignature = Request.Headers["Stripe-Signature"];

            try
            {
                var success = await _stripeService.HandleWebhookAsync(json, stripeSignature!);
                if (success)
                {
                    return Ok();
                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing Stripe webhook");
                return BadRequest();
            }
        }
    }

    public class ConfirmPaymentDTO
    {
        public string SessionId { get; set; } = string.Empty;
    }
}
