using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RoomReservationApiNet.DTOs;
using RoomReservationApiNet.Services;

namespace RoomReservationApiNet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "admin")]
    public class EmailQueueController : ControllerBase
    {
        private readonly IEmailQueueService _emailQueueService;

        public EmailQueueController(IEmailQueueService emailQueueService)
        {
            _emailQueueService = emailQueueService;
        }

        // GET: api/EmailQueue
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EmailQueueDTO>>> GetEmailQueues()
        {
            return Ok(await _emailQueueService.GetAllEmailQueues());
        }

        // GET: api/EmailQueue/5
        [HttpGet("{id}")]
        public async Task<ActionResult<EmailQueueDTO>> GetEmailQueue(int id)
        {
            var emailQueue = await _emailQueueService.GetEmailQueue(id);
            if (emailQueue.Result == null)
            {
                return NotFound();
            }

            return emailQueue;
        }

        // PUT: api/EmailQueue/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEmailQueue(int id, UpdateEmailQueueDTO updateEmailQueueDto)
        {
            return await _emailQueueService.PutEmailQueue(id, updateEmailQueueDto);
        }

        // POST: api/EmailQueue
        [HttpPost]
        public async Task<ActionResult<EmailQueueDTO>> PostEmailQueue(CreateEmailQueueDTO createEmailQueueDto)
        {
            return await _emailQueueService.PostEmailQueue(createEmailQueueDto);
        }

        // DELETE: api/EmailQueue/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmailQueue(int id)
        {
            return await _emailQueueService.DeleteEmailQueue(id);
        }
    }
}
