using System.ComponentModel.DataAnnotations;

namespace ClinicFlow_Backend.Model
{
    public class AuditLog
    {
        [Key]
        public Guid AuditID { get; set; } = Guid.NewGuid();

        [Required]
        public Guid UserID { get; set; }

        [Required, MaxLength(100)]
        public string Action { get; set; } = string.Empty;
        // e.g. CREATE_APPOINTMENT, UPDATE_PATIENT

        [Required, MaxLength(50)]
        public string ResourceType { get; set; } = string.Empty;
        // e.g. Appointment, Patient

        [MaxLength(50)]
        public string? ResourceID { get; set; }

        public string? Details { get; set; }
        // JSON payload with before/after state

        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
        // Immutable - no UPDATE or DELETE allowed on this table

    }
}
