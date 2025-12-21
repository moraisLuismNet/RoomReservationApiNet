using System.ComponentModel.DataAnnotations;

namespace RoomReservationApiNet.DTOs
{
  public class CreateReservationStatusDTO
  {
    [Required]
    [StringLength(50)]
    public required string Name { get; set; }
  }
}
