using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RoomReservationApiNet.Models
{
    public class RoomType
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RoomTypeId { get; set; }

        [Required]
        [StringLength(100)]
        public required string RoomTypeName { get; set; }

        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal PricePerNight { get; set; }

        [StringLength(500)]
        public string Description { get; set; } = string.Empty;

        public int Capacity { get; set; } = 2;
    }
}
