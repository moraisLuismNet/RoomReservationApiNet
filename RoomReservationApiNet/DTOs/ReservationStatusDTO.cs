using System.ComponentModel.DataAnnotations;

namespace RoomReservationApiNet.DTOs
{
  public class ReservationStatusDTO
  {
    public int StatusId { get; set; }

    [Required]
    [StringLength(50)]
    public required string Name { get; set; }
  }
}
