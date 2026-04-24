using ClinicFlow_Backend.DTO;
using ClinicFlow_Backend.Model;
using ClinicFlow_Backend.Repositories.Interface;
using Microsoft.AspNetCore.Mvc;

namespace ClinicFlow_Backend.Controllers
{
    [ApiController]
    [Route("api/v1/tasks")]
    public class TaskController : ControllerBase
    {
        private readonly ITaskRepository _repo;

        public TaskController(ITaskRepository repo)
        {
            _repo = repo;
        }

        // GET api/v1/tasks?userId={userId}
        [HttpGet]
        public async Task<IActionResult> GetForUser([FromQuery] Guid userId)
        {
            try
            {
                if (userId == Guid.Empty)
                    return BadRequest(new { error = "userId is required." });

                var tasks = await _repo.GetTasksByAssigneeAsync(userId);
                return Ok(tasks.Select(Map));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        // POST api/v1/tasks
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateTaskDto dto)
        {
            try
            {
                var task = new ClinicTask
                {
                    AssignedTo      = dto.AssignedTo,
                    RelatedEntityID = dto.RelatedEntityID,
                    Description     = dto.Description,
                    DueDate         = dto.DueDate,
                    Priority        = dto.Priority,
                    Status          = "Open",
                    CreatedAt       = DateTime.UtcNow
                };

                var created = await _repo.PostTaskAsync(task);
                return CreatedAtAction(nameof(GetForUser), new { userId = created.AssignedTo }, Map(created));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        // PUT api/v1/tasks/{id}
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateTaskDto dto)
        {
            try
            {
                var task = await _repo.GetTaskAsync(id);
                if (task is null)
                    return NotFound(new { error = $"Task '{id}' not found." });

                task.AssignedTo      = dto.AssignedTo;
                task.RelatedEntityID = dto.RelatedEntityID;
                task.Description     = dto.Description;
                task.DueDate         = dto.DueDate;
                task.Priority        = dto.Priority;
                task.Status          = dto.Status;
                task.CompletedAt     = dto.CompletedAt;

                await _repo.PutTaskAsync(id, task);
                return Ok(Map(task));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        // PATCH api/v1/tasks/{id}/complete
        [HttpPatch("{id:guid}/complete")]
        public async Task<IActionResult> Complete(Guid id)
        {
            try
            {
                var task = await _repo.GetTaskAsync(id);
                if (task is null)
                    return NotFound(new { error = $"Task '{id}' not found." });

                if (task.Status == "Completed")
                    return Conflict(new { error = $"Task '{id}' is already completed." });

                task.Status      = "Completed";
                task.CompletedAt = DateTime.UtcNow;

                await _repo.PutTaskAsync(id, task);
                return Ok(Map(task));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        private static TaskDto Map(ClinicTask t) => new()
        {
            TaskID          = t.TaskID,
            AssignedTo      = t.AssignedTo,
            RelatedEntityID = t.RelatedEntityID,
            Description     = t.Description,
            DueDate         = t.DueDate,
            Priority        = t.Priority,
            CreatedAt       = t.CreatedAt,
            CompletedAt     = t.CompletedAt,
            Status          = t.Status
        };
    }
}
