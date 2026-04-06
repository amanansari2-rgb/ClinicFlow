using System.ComponentModel.DataAnnotations;

namespace ClinicFlow_Backend.Model
{
    public class Identity
    {
        [Key]
        public Guid UserID { get; set; } = Guid.NewGuid();

        [Required, MaxLength(150)]
        public string Name { get; set; } = string.Empty;

        [Required, MaxLength(30)]
        public string Role { get; set; } = string.Empty;
        // Allowed: Patient | Clinician | Scheduler | Billing | Admin | Auditor

        [Required, MaxLength(200)]
        public string Email { get; set; } = string.Empty;

        [Required, MaxLength(500)]
        public string PasswordHash { get; set; } = string.Empty;

        [MaxLength(20)]
        public string? Phone { get; set; }

        [Required, MaxLength(20)]
        public string Status { get; set; } = "Active";
        // Allowed: Active | Inactive | Locked

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}