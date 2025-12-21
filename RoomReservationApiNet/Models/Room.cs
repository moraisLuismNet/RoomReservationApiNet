using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RoomReservationApiNet.Models
{
    public class Room
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RoomId { get; set; }

        [Required]
        [StringLength(10)]
        public required string RoomNumber { get; set; }

        [Required]
        public int RoomTypeId { get; set; }

        [ForeignKey("RoomTypeId")]
        public required RoomType RoomType { get; set; }

        public bool IsActive { get; set; } = true;
				
        public string? ImageRoom { get; set; }
    }
}
