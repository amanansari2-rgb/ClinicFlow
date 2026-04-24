using System.Security.Claims;
using System.Text.RegularExpressions;
using ClinicFlow_Backend.DTO;
using ClinicFlow_Backend.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ClinicFlow_Backend.Controllers
{
    [ApiController]
    [Route("auth")]
    public class AuthController : ControllerBase
    {
        private static readonly string[] AllowedGenders =
            { "Male", "Female", "Other", "Prefer Not To Say" };

        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        // POST /auth/register — public, always creates a Patient account
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto? dto)
        {
            if (dto == null)
                return BadRequest(new { message = "Request body is required." });

            // ── Name ──────────────────────────────────────────────────────
            if (string.IsNullOrWhiteSpace(dto.Name))
                return BadRequest(new { message = "Name is required." });

            if (dto.Name.Trim().Length < 2)
                return BadRequest(new { message = "Name must be at least 2 characters." });

            // ── Email ─────────────────────────────────────────────────────
            if (string.IsNullOrWhiteSpace(dto.Email))
                return BadRequest(new { message = "Email is required." });

            if (!Regex.IsMatch(dto.Email.Trim(), @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                return BadRequest(new { message = "Email format is invalid." });

            // ── Password ──────────────────────────────────────────────────
            if (string.IsNullOrWhiteSpace(dto.Password))
                return BadRequest(new { message = "Password is required." });

            if (dto.Password.Length < 6)
                return BadRequest(new { message = "Password must be at least 6 characters." });

            // ── DOB ───────────────────────────────────────────────────────
            if (dto.DOB == default)
                return BadRequest(new { message = "Date of birth is required." });

            if (dto.DOB >= DateOnly.FromDateTime(DateTime.UtcNow))
                return BadRequest(new { message = "Date of birth must be in the past." });

            if (dto.DOB.Year < 1900)
                return BadRequest(new { message = "Date of birth is not valid." });

            // ── Gender ────────────────────────────────────────────────────
            if (string.IsNullOrWhiteSpace(dto.Gender))
                return BadRequest(new { message = "Gender is required." });

            if (!AllowedGenders.Contains(dto.Gender.Trim(),
                    StringComparer.OrdinalIgnoreCase))
                return BadRequest(new
                {
                    message = $"Gender must be one of: {string.Join(", ", AllowedGenders)}."
                });

            // ── Normalise before passing to service ───────────────────────
            dto.Email  = dto.Email.Trim().ToLower();
            dto.Name   = dto.Name.Trim();
            dto.Gender = dto.Gender.Trim();

            try
            {
                var result = await _authService.RegisterAsync(dto);
                return CreatedAtAction(nameof(Register), result);
            }
            catch (InvalidOperationException ex)
            {
                // Email already taken
                return Conflict(new { message = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "An unexpected error occurred. Please try again." });
            }
        }

        // POST /auth/login — public
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto? dto)
        {
            if (dto == null)
                return BadRequest(new { message = "Request body is required." });

            if (string.IsNullOrWhiteSpace(dto.Email))
                return BadRequest(new { message = "Email is required." });

            if (!Regex.IsMatch(dto.Email.Trim(), @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                return BadRequest(new { message = "Email format is invalid." });

            if (string.IsNullOrWhiteSpace(dto.Password))
                return BadRequest(new { message = "Password is required." });

            if (dto.Password.Length < 6)
                return BadRequest(new { message = "Password must be at least 6 characters." });

            dto.Email = dto.Email.Trim().ToLower();

            try
            {
                var result = await _authService.LoginAsync(dto);
                return Ok(result);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "An unexpected error occurred. Please try again." });
            }
        }

        // POST /auth/logout — requires valid JWT
        [HttpPost("logout")]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            try
            {
                await _authService.LogoutAsync();
                return NoContent();
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "An unexpected error occurred during logout." });
            }
        }

        // GET /auth/me — reads current user from JWT claims, no DB call
        [HttpGet("me")]
        [Authorize]
        public IActionResult Me()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var email  = User.FindFirst(ClaimTypes.Email)?.Value;
            var role   = User.FindFirst(ClaimTypes.Role)?.Value;
            var name   = User.FindFirst(ClaimTypes.Name)?.Value;

            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(email))
                return Unauthorized(new { message = "Token is invalid or malformed." });

            return Ok(new { userId, email, role, name });
        }
    }
}
