using System.ComponentModel.DataAnnotations;

namespace RoomReservationApiNet.DTOs
{
  public class RoomDTO
  {
    public int RoomId { get; set; }

    [Required]
    [StringLength(10)]
    public required string RoomNumber { get; set; }

    [Required]
    public int RoomTypeId { get; set; }

    public string? RoomTypeName { get; set; }

    public decimal PricePerNight { get; set; }

    public string? Description { get; set; }

    public int Capacity { get; set; }

    public bool IsActive { get; set; }
		
    public string? ImageRoom { get; set; }
  }
}
