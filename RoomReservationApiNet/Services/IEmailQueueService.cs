using Microsoft.AspNetCore.Mvc;
using RoomReservationApiNet.DTOs;

namespace RoomReservationApiNet.Services
{
    public interface IEmailQueueService
    {
        Task<ActionResult<IEnumerable<EmailQueueDTO>>> GetAllEmailQueues();
        Task<ActionResult<EmailQueueDTO>> GetEmailQueue(int id);
        Task<IActionResult> PutEmailQueue(int id, UpdateEmailQueueDTO updateEmailQueueDto);
        Task<ActionResult<EmailQueueDTO>> PostEmailQueue(CreateEmailQueueDTO createEmailQueueDto);
        Task<IActionResult> DeleteEmailQueue(int id);
    }
}
