using System.ComponentModel.DataAnnotations;

namespace RoomReservationApiNet.DTOs
{
  public class UpdateRoomDTO
  {
    [Required]
    [StringLength(10)]
    public required string RoomNumber { get; set; }

    [Required]
    public int RoomTypeId { get; set; }

    public bool IsActive { get; set; }
		
    public string? ImageRoom { get; set; }
  }
}
