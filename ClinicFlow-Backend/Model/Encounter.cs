using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClinicFlow_Backend.Model
{
    public class Encounter
    {
        [Key]
        public Guid EncounterID { get; set; } = Guid.NewGuid();

        [Required]
        public Guid AppID { get; set; }
        // FK → Appointment

        [Required]
        public Guid PatientID { get; set; }
        // FK → Patient

        [Required]
        public Guid ProviderID { get; set; }
        // FK → Provider

        [Required]
        public DateTime StartAt { get; set; }

        public DateTime? EndAt { get; set; }
        // Null while encounter is still in progress

        [Required, MaxLength(30)]
        public string VisitType { get; set; } = string.Empty;
        // Routine | Urgent | FollowUp | Telehealth

        [Required, MaxLength(500)]
        public string ChiefComplaint { get; set; } = string.Empty;

        [MaxLength(500)]
        public string? EncounterNoteURI { get; set; }
        // Object storage URI for the SOAP note PDF

        [Required, MaxLength(20)]
        public string Status { get; set; } = "InProgress";
        // InProgress | Closed | Cancelled

        // Navigation
        [ForeignKey(nameof(AppID))]
        public Appointment Appointment { get; set; } = null!;

        [ForeignKey(nameof(PatientID))]
        public Patient Patient { get; set; } = null!;

        [ForeignKey(nameof(ProviderID))]
        public Provider Provider { get; set; } = null!;

        public ICollection<Order> Orders { get; set; } = [];
        public ICollection<Referral> Referrals { get; set; } = [];
        public ICollection<Charge> Charges { get; set; } = [];
    }
}
