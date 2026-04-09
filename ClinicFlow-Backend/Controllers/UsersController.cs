using ClinicFlow_Backend.Data;
using ClinicFlow_Backend.DTO;
using ClinicFlow_Backend.Model;
using ClinicFlow_Backend.Repositories.Interface;
using Humanizer;
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
            var Users =  await _repository.GetUsersAsync();
            return Ok(Users.Select(u => MapToDto(u)));
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(Guid id)
        {
            var user = await _repository.GetUserAsync(id);

            if (user == null)
                return NotFound();

            return Ok(MapToDto(user));
        }

        // PUT: api/Users/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(Guid id, UpdateUserDto dto)
        {
            var existing = await _repository.GetUserAsync(id);
            if (existing == null) return NotFound();

            existing.Name = dto.Name;
            existing.Email = dto.Email;
            existing.Phone = dto.Phone;
            existing.Status = dto.Status;
            existing.UpdatedAt = DateTime.UtcNow;

            var result = await _repository.PutUserAsync(id, existing);

            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }

        // POST: api/Users
        [HttpPost]
        public async Task<ActionResult<User>> PostUser(CreateUserDto dto)
        {
            var user = new User
            {
                Name = dto.Name,
                Role = dto.Role,
                Email = dto.Email,
                Phone = dto.Phone,
                PasswordHash = dto.Password, // TODO: BCrypt.HashPassword() in Week 2
                Status = "Active",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
            await _repository.PostUserAsync(user);

            return CreatedAtAction(nameof(GetUser), new { id = user.UserID }, MapToDto(user));
        }

        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            var result = await _repository.DeleteUserAsync(id);

            if(!result)
            {
                return NotFound();
            }

            return NoContent();
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
