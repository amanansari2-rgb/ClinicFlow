using System.ComponentModel.DataAnnotations;

namespace ClinicFlow_Backend.DTO
{
    // ─── RESPONSE DTO ─────────────────────────────────────────────────────────
    public class UserDto
    {
        public Guid UserID { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? Phone { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    // ─── CREATE REQUEST DTO ───────────────────────────────────────────────────
    public class CreateUserDto
    {
        [Required, MaxLength(150)]
        public string Name { get; set; } = string.Empty;

        [Required, MaxLength(30)]
        public string Role { get; set; } = string.Empty;

        [Required, EmailAddress, MaxLength(200)]
        public string Email { get; set; } = string.Empty;

        [Required, MinLength(6)]
        public string Password { get; set; } = string.Empty;

        [Phone, MaxLength(20)]
        public string? Phone { get; set; }
    }

    // ─── UPDATE REQUEST DTO ───────────────────────────────────────────────────
    public class UpdateUserDto
    {
        [Required, MaxLength(150)]
        public string Name { get; set; } = string.Empty;

        [Required, EmailAddress, MaxLength(200)]
        public string Email { get; set; } = string.Empty;

        [Phone, MaxLength(20)]
        public string? Phone { get; set; }

        [Required, MaxLength(20)]
        public string Status { get; set; } = string.Empty;
    }
}
