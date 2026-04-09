namespace ClinicFlow_Backend.DTO
{
    // ─── RESPONSE DTO ─────────────────────────────────────────────────────────
    // Returned by GET /users and GET /users/{id}
    // Never exposes PasswordHash
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
    // Used by POST /users
    // Accepts plain password — service layer will hash it before saving
    public class CreateUserDto
    {
        public string Name { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;   // Patient | Clinician | Scheduler | Billing | Admin | Auditor
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string? Phone { get; set; }
    }

    // ─── UPDATE REQUEST DTO ───────────────────────────────────────────────────
    // Used by PUT /users/{id}
    // Does not allow role or password change here — those are separate operations
    public class UpdateUserDto
    {
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? Phone { get; set; }
        public string Status { get; set; } = string.Empty; // Active | Inactive | Locked
    }
}
