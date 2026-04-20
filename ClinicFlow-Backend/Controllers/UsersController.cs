using ClinicFlow_Backend.DTO;
using ClinicFlow_Backend.Model;
using ClinicFlow_Backend.Repositories.Interface;
using Microsoft.AspNetCore.Mvc;

namespace ClinicFlow_Backend.Controllers
{
    [ApiController]
    [Route("api/v1/users")]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _repository;

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
                return Ok(users.Select(MapToDto));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        // GET api/v1/users/{id}
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetUser(Guid id)
        {
            try
            {
                var user = await _repository.GetUserAsync(id);
                if (user is null)
                    return NotFound(new { error = $"User '{id}' not found." });

                return Ok(MapToDto(user));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        // POST api/v1/users
        [HttpPost]
        public async Task<IActionResult> PostUser([FromBody] CreateUserDto dto)
        {
            try
            {
                var user = new User
                {
                    Name         = dto.Name,
                    Role         = dto.Role,
                    Email        = dto.Email,
                    Phone        = dto.Phone,
                    PasswordHash = dto.Password, // TODO: BCrypt.HashPassword()
                    Status       = "Active",
                    CreatedAt    = DateTime.UtcNow,
                    UpdatedAt    = DateTime.UtcNow
                };

                await _repository.PostUserAsync(user);
                return CreatedAtAction(nameof(GetUser), new { id = user.UserID }, MapToDto(user));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        // PUT api/v1/users/{id}
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> PutUser(Guid id, [FromBody] UpdateUserDto dto)
        {
            try
            {
                var existing = await _repository.GetUserAsync(id);
                if (existing is null)
                    return NotFound(new { error = $"User '{id}' not found." });

                existing.Name      = dto.Name;
                existing.Email     = dto.Email;
                existing.Phone     = dto.Phone;
                existing.Status    = dto.Status;
                existing.UpdatedAt = DateTime.UtcNow;

                await _repository.PutUserAsync(id, existing);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        // DELETE api/v1/users/{id}
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            try
            {
                var result = await _repository.DeleteUserAsync(id);
                if (!result)
                    return NotFound(new { error = $"User '{id}' not found." });

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
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
