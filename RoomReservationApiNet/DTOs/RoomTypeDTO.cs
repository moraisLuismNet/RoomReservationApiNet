using System.ComponentModel.DataAnnotations;

namespace RoomReservationApiNet.DTOs
{
  public class RoomTypeDTO
  {
    public int RoomTypeId { get; set; }

    [Required]
    [StringLength(100)]
    public required string RoomTypeName { get; set; }

    [Required]
    public decimal PricePerNight { get; set; }

    [StringLength(500)]
    public string? Description { get; set; }

    public int Capacity { get; set; }
  }
}
