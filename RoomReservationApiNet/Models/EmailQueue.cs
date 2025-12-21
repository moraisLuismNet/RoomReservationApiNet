using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RoomReservationApiNet.Models
{
    public class EmailQueue
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int EmailQueueId { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(255)]
        public required string ToEmail { get; set; }

        [Required]
        [StringLength(255)]
        public required string Subject { get; set; }

        [Required]
        public required string Body { get; set; }

        [Required]
        [StringLength(50)]
        public required string EmailType { get; set; }

        [StringLength(20)]
        public string Status { get; set; } = "pending";

        public int Attempts { get; set; } = 0;

        public int MaxAttempts { get; set; } = 3;

        [Required]
        public DateTime ScheduledSendTime { get; set; }

        public DateTime? SentAt { get; set; }

        [StringLength(1000)]
        public string ErrorMessage { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public int? ReservationId { get; set; }

        [ForeignKey("ReservationId")]
        public Reservation? Reservation { get; set; }

        public string? Metadata { get; set; }
    }
}
