using System.Text;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using RoomReservationApiNet.Data;
using RoomReservationApiNet.Models;

namespace RoomReservationApiNet.Services
{
    public class EmailService
    {
        private readonly AppDbContext _context;
        private readonly EmailConfiguration _emailConfig;
        private readonly HttpClient _httpClient;

        public EmailService(AppDbContext context, EmailConfiguration emailConfig, HttpClient httpClient)
        {
            _context = context;
            _emailConfig = emailConfig;
            _httpClient = httpClient;
        }

        public async Task SendEmailAsync(string toEmail, string subject, string body, string emailType, int? reservationId = null)
        {
            var emailQueue = new EmailQueue
            {
                ToEmail = toEmail,
                Subject = subject,
                Body = body,
                EmailType = emailType,
                Status = "pending",
                ScheduledSendTime = DateTime.UtcNow,
                ReservationId = reservationId,
                CreatedAt = DateTime.UtcNow,
                ErrorMessage = string.Empty,
                Metadata = string.Empty,
                Reservation = null
            };

            _context.EmailQueues.Add(emailQueue);
            await _context.SaveChangesAsync();

            // Process the email queue
            await ProcessEmailQueue();
        }

        private async Task ProcessEmailQueue()
        {
            var pendingEmails = await _context.EmailQueues
                .Where(e => e.Status == "pending" && e.ScheduledSendTime <= DateTime.UtcNow)
                .ToListAsync();

            foreach (var email in pendingEmails)
            {
                try
                {
                    await SendEmail(email);
                    email.Status = "sent";
                    email.SentAt = DateTime.UtcNow;
                }
                catch (Exception ex)
                {
                    email.Status = "failed";
                    email.ErrorMessage = ex.Message;
                    email.Attempts++;

                    if (email.Attempts < email.MaxAttempts)
                    {
                        email.Status = "retrying";
                        email.ScheduledSendTime = DateTime.UtcNow.AddMinutes(5);
                    }
                }
            }

            await _context.SaveChangesAsync();
        }

        private async Task SendEmail(EmailQueue email)
        {
            var request = new
            {
                sender = new { email = _emailConfig.FromEmail, name = _emailConfig.FromName },
                to = new[] { new { email = email.ToEmail } },
                subject = email.Subject,
                htmlContent = email.Body
            };

            var json = JsonSerializer.Serialize(request);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            _httpClient.DefaultRequestHeaders.Add("api-key", _emailConfig.BrevoApiKey);
            _httpClient.DefaultRequestHeaders.Add("accept", "application/json");

            var response = await _httpClient.PostAsync("https://api.brevo.com/v3/smtp/email", content);

            if (!response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                throw new Exception($"Failed to send email: {response.StatusCode} - {responseContent}");
            }
        }
    }

    public class EmailConfiguration
    {
        public required string FromEmail { get; set; }
        public required string FromName { get; set; }
        public required string BrevoApiKey { get; set; }
    }
}
