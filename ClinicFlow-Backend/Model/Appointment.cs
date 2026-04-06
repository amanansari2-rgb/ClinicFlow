using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClinicFlow_Backend.Model
{
    public class Appointment
    {
        [Key]
        public Guid AppID { get; set; } = Guid.NewGuid();

        [Required]
        public Guid PatientID { get; set; }
        // FK → Patient

        [Required]
        public Guid ProviderID { get; set; }
        // FK → Provider

        [Required]
        public Guid ClinicID { get; set; }
        // FK → Clinic

        [Required]
        public DateTime ScheduledAt { get; set; }
        // UTC

        public int DurationMinutes { get; set; } = 30;

        [Required, MaxLength(20)]
        public string Mode { get; set; } = string.Empty;
        // InPerson | Telehealth

        [Required, MaxLength(300)]
        public string Reason { get; set; } = string.Empty;

        [Required, MaxLength(20)]
        public string Status { get; set; } = "Scheduled";
        // Scheduled | Confirmed | CheckedIn | Completed | Cancelled | NoShow

        [Required]
        public Guid CreatedBy { get; set; }
        // FK → Identity

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation
        [ForeignKey(nameof(PatientID))]
        public Patient Patient { get; set; } = null!;

        [ForeignKey(nameof(ProviderID))]
        public Provider Provider { get; set; } = null!;

        [ForeignKey(nameof(ClinicID))]
        public Clinic Clinic { get; set; } = null!;

        public Encounter? Encounter { get; set; }
    }
}
