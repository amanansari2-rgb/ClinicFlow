using ClinicFlow_Backend.DTO;
using ClinicFlow_Backend.Model;
using ClinicFlow_Backend.Repositories.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ClinicFlow_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _repository;

        public UsersController(IUserRepository Repository)
        {
            _repository = Repository;
        }

        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetUsers()
        {
            try
            {
                var users = await _repository.GetUsersAsync();
                return Ok(users.Select(u => MapToDto(u)));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Failed to retrieve users.", detail = ex.Message });
            }
        }

        // GET: api/Users/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<UserDto>> GetUser(Guid id)
        {
            if (id == Guid.Empty)
                return BadRequest(new { error = "A valid User ID is required." });

            try
            {
                var user = await _repository.GetUserAsync(id);
                if (user == null)
                    return NotFound(new { error = $"User with ID '{id}' not found." });

                return Ok(MapToDto(user));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Failed to retrieve the user.", detail = ex.Message });
            }
        }

        // POST: api/Users
        [HttpPost]
        public async Task<ActionResult<UserDto>> PostUser(CreateUserDto dto)
        {
            try
            {
                var user = new User
                {
                    Name = dto.Name.Trim(),
                    Role = dto.Role.Trim(),
                    Email = dto.Email.Trim(),
                    Phone = dto.Phone?.Trim(),
                    PasswordHash = dto.Password, // TODO: BCrypt.HashPassword() in Week 2
                    Status = "Active",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                await _repository.PostUserAsync(user);
                return CreatedAtAction(nameof(GetUser), new { id = user.UserID }, MapToDto(user));
            }
            catch (DbUpdateException ex)
            {
                return Conflict(new { error = "Could not create user. Possible duplicate email.", detail = ex.InnerException?.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Failed to create user.", detail = ex.Message });
            }
        }

        // PUT: api/Users/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(Guid id, UpdateUserDto dto)
        {
            if (id == Guid.Empty)
                return BadRequest(new { error = "A valid User ID is required." });

            try
            {
                var existing = await _repository.GetUserAsync(id);
                if (existing == null)
                    return NotFound(new { error = $"User with ID '{id}' not found." });

                existing.Name = dto.Name.Trim();
                existing.Email = dto.Email.Trim();
                existing.Phone = dto.Phone?.Trim();
                existing.Status = dto.Status.Trim();
                existing.UpdatedAt = DateTime.UtcNow;

                var result = await _repository.PutUserAsync(id, existing);
                if (!result)
                    return StatusCode(500, new { error = "Failed to update the user." });

                return NoContent();
            }
            catch (DbUpdateException ex)
            {
                return Conflict(new { error = "Could not update user. Possible duplicate email or concurrency conflict.", detail = ex.InnerException?.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Failed to update the user.", detail = ex.Message });
            }
        }

        // DELETE: api/Users/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            if (id == Guid.Empty)
                return BadRequest(new { error = "A valid User ID is required." });

            try
            {
                var result = await _repository.DeleteUserAsync(id);
                if (!result)
                    return NotFound(new { error = $"User with ID '{id}' not found." });

                return NoContent();
            }
            catch (DbUpdateException ex)
            {
                return Conflict(new { error = "Cannot delete — user is linked to other records.", detail = ex.InnerException?.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Failed to delete the user.", detail = ex.Message });
            }
        }

        private static UserDto MapToDto(User user) => new UserDto
        {
            UserID = user.UserID,
            Name = user.Name,
            Role = user.Role,
            Email = user.Email,
            Phone = user.Phone,
            Status = user.Status,
            CreatedAt = user.CreatedAt,
            UpdatedAt = user.UpdatedAt,
        };
    }
}
