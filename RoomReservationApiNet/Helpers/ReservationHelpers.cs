using RoomReservationApiNet.DTOs;
using RoomReservationApiNet.Models;
using RoomReservationApiNet.Repository;

namespace RoomReservationApiNet.Helpers
{
  public static class ReservationHelpers
  {
    // Helper to convert Reservation to ReservationDTO
    public static ReservationDTO ConvertToReservationDTO(Reservation reservation)
    {
      return new ReservationDTO
      {
        ReservationId = reservation.ReservationId,
        StatusId = reservation.StatusId,
        User = new UserDTO
        {
          Email = reservation.User.Email,
          FullName = reservation.User.FullName
        },
        RoomId = reservation.RoomId,
        ReservationDate = reservation.ReservationDate,
        CheckInDate = reservation.CheckInDate,
        CheckOutDate = reservation.CheckOutDate,
        NumberOfNights = reservation.NumberOfNights,
        NumberOfGuests = reservation.NumberOfGuests,
        CancellationDate = reservation.CancellationDate,
        CancellationReason = reservation.CancellationReason
      };
    }

    // Helper to check room availability
    public static async Task<bool> IsRoomAvailable(int roomId, DateTime checkInDate, DateTime checkOutDate, IReservationRepository reservationRepository)
    {
      return await reservationRepository.IsRoomAvailable(roomId, checkInDate, checkOutDate);
    }

    // Helper to determine if a reservation can be canceled
    public static bool CanCancelReservation(Reservation reservation)
    {
      // Check if the reservation can be cancelled (it is not in cancelled or checked-in status)
      if (reservation.StatusId == 5 || reservation.StatusId == 3) // 5 = cancelled, 3 = checked-in
      {
        return false;
      }

      // Check if the cancellation is being made at least 24 hours before check-in
      var now = DateTime.UtcNow;
      var timeUntilCheckIn = reservation.CheckInDate - now;
      if (timeUntilCheckIn.TotalHours < 24)
      {
        return false;
      }

      return true;
    }

    // Helper to generate the body of the booking confirmation email
    public static string GenerateConfirmationEmailBody(User user, Reservation reservation)
    {
      return $@"
                <h1>Booking Confirmation</h1>
                <p>Dear {user.FullName},</p>
                <p>Your booking has been confirmed successfully.</p>
                <p><strong>Booking Details:</strong></p>
                <ul>
                    <li>Entry date: {reservation.CheckInDate:dd/MM/yyyy}</li>
                    <li>Departure date: {reservation.CheckOutDate:dd/MM/yyyy}</li>
                    <li>Number of nights: {reservation.NumberOfNights}</li>
                    <li>Number of guests: {reservation.NumberOfGuests}</li>
                </ul>
                <p>If you have any questions, please do not hesitate to contact us.</p>
            ";
    }

    // Helper to generate the body of the cancellation confirmation email
    public static string GenerateCancellationEmailBody(User user, Reservation reservation)
    {
      return $@"
                <h1>Reservation Cancellation Confirmation</h1>
                <p>Dear {user.FullName},</p>
                <p>Your reservation has been successfully cancelled.</p>
                <p><strong>Reservation Details:</strong></p>
                <ul>
                    <li>Entry date: {reservation.CheckInDate:dd/MM/yyyy}</li>
                    <li>Departure date: {reservation.CheckOutDate:dd/MM/yyyy}</li>
                    <li>Number of nights: {reservation.NumberOfNights}</li>
                    <li>Number of guests: {reservation.NumberOfGuests}</li>
                </ul>
                <p>If you have any questions, please do not hesitate to contact us.</p>
            ";
    }

    // Helper to check if a user has permission to access or modify a reservation
    public static bool HasUserPermission(string userEmail, string requestedEmail, bool isAdmin)
    {
      return isAdmin || userEmail == requestedEmail;
    }

    // Helper to calculate the number of nights between two dates
    public static int CalculateNumberOfNights(DateTime checkInDate, DateTime checkOutDate)
    {
      return (int)(checkOutDate - checkInDate).TotalDays;
    }

    // Helper to generate error messages in Spanish
    public static string GenerateErrorMessage(string errorType)
    {
      switch (errorType)
      {
        case "roomNotAvailable":
          return "The room is not available for the selected dates.";
        case "reservationNotFound":
          return "The reservation was not found.";
        case "cannotCancel":
          return "The reservation cannot be cancelled because it is already cancelled.";
        case "cancelationTimeExceeded":
          return "The reservation cannot be cancelled because the cancellation time has been exceeded.";
        case "statusNotFound":
          return "The reservation status was not found.";
        default:
          return "An unexpected error occurred.";
      }
    }

    // Helper to convert ReservationStatus to ReservationStatusDTO
    public static ReservationStatusDTO ConvertToReservationStatusDTO(ReservationStatus reservationStatus)
    {
      return new ReservationStatusDTO
      {
        StatusId = reservationStatus.StatusId,
        Name = reservationStatus.Name
      };
    }

    // Helper to validate the existence of a reservation status
    public static bool IsReservationStatusValid(int statusId, IReservationStatusRepository reservationStatusRepository)
    {
      var status = reservationStatusRepository.GetReservationStatusById(statusId);
      return status != null;
    }

    // Helper to convert Room to RoomDTO
    public static RoomDTO ConvertToRoomDTO(Room room)
    {
      return new RoomDTO
      {
        RoomId = room.RoomId,
        RoomNumber = room.RoomNumber,
        RoomTypeId = room.RoomTypeId,
        RoomTypeName = room.RoomType.RoomTypeName,
        PricePerNight = room.RoomType.PricePerNight,
        Description = room.RoomType.Description,
        Capacity = room.RoomType.Capacity,
        IsActive = room.IsActive
      };
    }

    // Helper to validate the existence of a room
    public static bool IsRoomValid(int roomId, IRoomRepository roomRepository)
    {
      var room = roomRepository.GetRoomById(roomId);
      return room != null;
    }

    // Helper to convert RoomType to RoomTypeDTO
    public static RoomTypeDTO ConvertToRoomTypeDTO(RoomType roomType)
    {
      return new RoomTypeDTO
      {
        RoomTypeId = roomType.RoomTypeId,
        RoomTypeName = roomType.RoomTypeName,
        PricePerNight = roomType.PricePerNight,
        Description = roomType.Description,
        Capacity = roomType.Capacity
      };
    }

    // Helper to validate the existence of a room type
    public static bool IsRoomTypeValid(int roomTypeId, IRoomTypeRepository roomTypeRepository)
    {
      var roomType = roomTypeRepository.GetRoomTypeById(roomTypeId);
      return roomType != null;
    }

    // Helper to convert User to UserDTO
    public static UserDTO ConvertToUserDTO(User user)
    {
      return new UserDTO
      {
        Email = user.Email,
        FullName = user.FullName,
        PhoneNumber = user.PhoneNumber,
        IsActive = user.IsActive,
        CreatedAt = user.CreatedAt,
        LastLogin = user.LastLogin,
        Role = user.Role
      };
    }

    // Helper to validate the existence of a user
    public static bool IsUserValid(string email, IUserRepository userRepository)
    {
      var user = userRepository.GetUserById(email);
      return user != null;
    }

    // Helper to hash a password using BCrypt
    public static string HashPassword(string password)
    {
      return BCrypt.Net.BCrypt.HashPassword(password);
    }
  }
}
