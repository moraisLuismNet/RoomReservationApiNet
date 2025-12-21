using System.ComponentModel.DataAnnotations;

namespace RoomReservationApiNet.DTOs
{
  public class UpdateReservationStatusDTO
  {
    [Required]
    [StringLength(50)]
    public required string Name { get; set; }
  }
}
