using Microsoft.AspNetCore.Mvc;
using RoomReservationApiNet.DTOs;
using RoomReservationApiNet.Models;
using RoomReservationApiNet.Repository;

namespace RoomReservationApiNet.Services
{
  public class RoomTypeService : IRoomTypeService
  {
    private readonly IRoomTypeRepository _roomTypeRepository;

    public RoomTypeService(IRoomTypeRepository roomTypeRepository)
    {
      _roomTypeRepository = roomTypeRepository;
    }

    public async Task<IEnumerable<RoomTypeDTO>> GetAllRoomTypes()
    {
      var roomTypes = await _roomTypeRepository.GetAllRoomTypes();
      var roomTypeDtos = roomTypes.Select(rt => new RoomTypeDTO
      {
        RoomTypeId = rt.RoomTypeId,
        RoomTypeName = rt.RoomTypeName,
        PricePerNight = rt.PricePerNight,
        Description = rt.Description,
        Capacity = rt.Capacity
      }).ToList();

      return roomTypeDtos;
    }

    public async Task<ActionResult<RoomTypeDTO>> GetRoomType(int id)
    {
      var roomType = await _roomTypeRepository.GetRoomTypeById(id);
      if (roomType == null)
      {
        return new NotFoundResult();
      }

      var roomTypeDto = new RoomTypeDTO
      {
        RoomTypeId = roomType.RoomTypeId,
        RoomTypeName = roomType.RoomTypeName,
        PricePerNight = roomType.PricePerNight,
        Description = roomType.Description,
        Capacity = roomType.Capacity
      };

      return roomTypeDto;
    }

    public async Task<IActionResult> PutRoomType(int id, UpdateRoomTypeDTO updateRoomTypeDto)
    {
      var existingRoomType = await _roomTypeRepository.GetRoomTypeById(id);
      if (existingRoomType == null)
      {
        return new NotFoundResult();
      }

      existingRoomType.RoomTypeName = updateRoomTypeDto.RoomTypeName;
      existingRoomType.PricePerNight = updateRoomTypeDto.PricePerNight;
      existingRoomType.Description = updateRoomTypeDto.Description ?? string.Empty;
      existingRoomType.Capacity = updateRoomTypeDto.Capacity;

      await _roomTypeRepository.UpdateRoomType(existingRoomType);
      return new NoContentResult();
    }

    public async Task<ActionResult<RoomTypeDTO>> PostRoomType(CreateRoomTypeDTO createRoomTypeDto)
    {
      var roomType = new RoomType
      {
        RoomTypeName = createRoomTypeDto.RoomTypeName,
        PricePerNight = createRoomTypeDto.PricePerNight,
        Description = createRoomTypeDto.Description ?? string.Empty,
        Capacity = createRoomTypeDto.Capacity
      };

      await _roomTypeRepository.AddRoomType(roomType);

      var roomTypeDto = new RoomTypeDTO
      {
        RoomTypeId = roomType.RoomTypeId,
        RoomTypeName = roomType.RoomTypeName,
        PricePerNight = roomType.PricePerNight,
        Description = roomType.Description,
        Capacity = roomType.Capacity
      };

      return roomTypeDto;
    }

    public async Task<IActionResult> DeleteRoomType(int id)
    {
      var roomType = await _roomTypeRepository.GetRoomTypeById(id);
      if (roomType == null)
      {
        return new NotFoundResult();
      }

      await _roomTypeRepository.DeleteRoomType(id);
      return new NoContentResult();
    }
  }
}
