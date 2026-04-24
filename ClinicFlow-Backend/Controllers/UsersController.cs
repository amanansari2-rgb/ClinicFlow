using ClinicFlow_Backend.DTO;
using ClinicFlow_Backend.Model;
using ClinicFlow_Backend.Repositories.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ClinicFlow_Backend.Controllers
{
    [Route("[controller]")]
    [ApiController]

    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _repository;

        private static readonly string[] AllowedRoles =
            { "Patient", "Clinician", "Scheduler", "Billing", "Admin", "Auditor" };
        private static readonly string[] AllowedStatuses =
            { "Active", "Inactive", "Locked" };

        public UsersController(IUserRepository repository)
        {
            _repository = repository;
        }

        // GET api/v1/users
        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            try
            {
                var users = await _repository.GetUsersAsync();
                return Ok(users.Select(u => MapToDto(u)));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An unexpected error occurred.", detail = ex.Message });
            }
        }

        // GET: api/Users/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<UserDto>> GetUser(Guid id)
        {
            try
            {
                var user = await _repository.GetUserAsync(id);

                if (user == null)
                    return NotFound(new { message = $"User with ID {id} was not found." });

                return Ok(MapToDto(user));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An unexpected error occurred.", detail = ex.Message });
            }
        }

        // POST: api/Users
        [HttpPost]
        public async Task<ActionResult<UserDto>> PostUser(CreateUserDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Name))
                return BadRequest(new { message = "Name is required." });

            if (string.IsNullOrWhiteSpace(dto.Email))
                return BadRequest(new { message = "Email is required." });

            if (string.IsNullOrWhiteSpace(dto.Password))
                return BadRequest(new { message = "Password is required." });

            var normalizedRole = AllowedRoles.FirstOrDefault(r => r.Equals(dto.Role, StringComparison.OrdinalIgnoreCase));
            if (string.IsNullOrWhiteSpace(dto.Role) || normalizedRole == null)
                return BadRequest(new { message = $"Role must be one of: {string.Join(", ", AllowedRoles)}." });

            try
            {
                var user = new User
                {
                    Name = dto.Name,
                    Role = normalizedRole,
                    Email = dto.Email,
                    Phone = dto.Phone,

                    PasswordHash = dto.Password, // TODO: hash with BCrypt in Week 2
                    Status = "Active",

                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                await _repository.PostUserAsync(user);
                return CreatedAtAction(nameof(GetUser), new { id = user.UserID }, MapToDto(user));
            }
            catch (DbUpdateException)
            {
                return Conflict(new { message = "A user with this email already exists." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An unexpected error occurred.", detail = ex.Message });
            }
        }

        // PUT: api/Users/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(Guid id, UpdateUserDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Name))
                return BadRequest(new { message = "Name is required." });

            if (string.IsNullOrWhiteSpace(dto.Email))
                return BadRequest(new { message = "Email is required." });

            if (string.IsNullOrWhiteSpace(dto.Status) || !AllowedStatuses.Contains(dto.Status))
                return BadRequest(new { message = $"Status must be one of: {string.Join(", ", AllowedStatuses)}." });

            try
            {
                var existing = await _repository.GetUserAsync(id);

                if (existing == null)
                    return NotFound(new { message = $"User with ID {id} was not found." });

                existing.Name = dto.Name;
                existing.Email = dto.Email;
                existing.Phone = dto.Phone;
                existing.Status = dto.Status;
                existing.UpdatedAt = DateTime.UtcNow;

                await _repository.PutUserAsync(id, existing);
                return NoContent();
            }
            catch (DbUpdateException)
            {
                return Conflict(new { message = "Update failed � email may already be in use." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An unexpected error occurred.", detail = ex.Message });
            }
        }

        // DELETE: api/Users/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            try
            {
                var result = await _repository.DeleteUserAsync(id);

                if (!result)
                    return NotFound(new { message = $"User with ID {id} was not found." });

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An unexpected error occurred.", detail = ex.Message });
            }
        }

        private static UserDto MapToDto(User u) => new()
        {
            UserID    = u.UserID,
            Name      = u.Name,
            Role      = u.Role,
            Email     = u.Email,
            Phone     = u.Phone,
            Status    = u.Status,
            CreatedAt = u.CreatedAt,
            UpdatedAt = u.UpdatedAt
        };
    }
}