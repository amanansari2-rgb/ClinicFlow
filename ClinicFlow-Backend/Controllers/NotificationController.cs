using ClinicFlow_Backend.DTO;
using ClinicFlow_Backend.Model;
using ClinicFlow_Backend.Repositories.Interface;
using Microsoft.AspNetCore.Mvc;

namespace ClinicFlow_Backend.Controllers
{
    [ApiController]
    [Route("api/v1/notifications")]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationRepository _repo;

        public NotificationController(INotificationRepository repo)
        {
            _repo = repo;
        }

        // GET api/v1/notifications?userId={userId}
        [HttpGet]
        public async Task<IActionResult> GetForUser([FromQuery] Guid userId)
        {
            try
            {
                if (userId == Guid.Empty)
                    return BadRequest(new { error = "userId is required." });

                var items = await _repo.GetNotificationsByUserAsync(userId);
                return Ok(items.Select(Map));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        // PATCH api/v1/notifications/{id}/read
        [HttpPatch("{id:guid}/read")]
        public async Task<IActionResult> MarkRead(Guid id)
        {
            try
            {
                var notification = await _repo.GetNotificationAsync(id);
                if (notification is null)
                    return NotFound(new { error = $"Notification '{id}' not found." });

                notification.Status = "Read";
                notification.ReadAt = DateTime.UtcNow;

                await _repo.PutNotificationAsync(id, notification);
                return Ok(Map(notification));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        // DELETE api/v1/notifications/{id}
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Dismiss(Guid id)
        {
            try
            {
                var notification = await _repo.GetNotificationAsync(id);
                if (notification is null)
                    return NotFound(new { error = $"Notification '{id}' not found." });

                notification.Status = "Dismissed";
                notification.ReadAt ??= DateTime.UtcNow;

                await _repo.PutNotificationAsync(id, notification);
                return Ok(Map(notification));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        private static NotificationDto Map(Notification n) => new()
        {
            NotificationID = n.NotificationID,
            UserID         = n.UserID,
            EntityID       = n.EntityID,
            Message        = n.Message,
            Category       = n.Category,
            Severity       = n.Severity,
            CreatedAt      = n.CreatedAt,
            ReadAt         = n.ReadAt,
            Status         = n.Status
        };
    }
}
