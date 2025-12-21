using RoomReservationApiNet.Data;
using RoomReservationApiNet.DTOs;
using RoomReservationApiNet.Models;

namespace RoomReservationApiNet.Helpers
{
  public static class EmailQueueHelpers
  {
    // Helper to convert EmailQueue to EmailQueueDTO
    public static EmailQueueDTO ConvertToEmailQueueDTO(EmailQueue emailQueue)
    {
      return new EmailQueueDTO
      {
        EmailQueueId = emailQueue.EmailQueueId,
        ToEmail = emailQueue.ToEmail,
        Subject = emailQueue.Subject,
        Body = emailQueue.Body,
        EmailType = emailQueue.EmailType,
        Status = emailQueue.Status,
        Attempts = emailQueue.Attempts,
        MaxAttempts = emailQueue.MaxAttempts,
        ScheduledSendTime = emailQueue.ScheduledSendTime,
        SentAt = emailQueue.SentAt,
        ErrorMessage = emailQueue.ErrorMessage,
        CreatedAt = emailQueue.CreatedAt,
        ReservationId = emailQueue.ReservationId,
        Metadata = emailQueue.Metadata
      };
    }

    // Helper to check if an EmailQueue exists by ID
    public static bool EmailQueueExists(int id, AppDbContext context)
    {
      return context.EmailQueues.Any(e => e.EmailQueueId == id);
    }
  }
}
