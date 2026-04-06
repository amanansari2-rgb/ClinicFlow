using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClinicFlow_Backend.Model
{
    public class Referral
    {
        [Key]
        public Guid ReferralID { get; set; } = Guid.NewGuid();

        [Required]
        public Guid EncounterID { get; set; }
        // FK → Encounter

        [Required]
        public Guid FromProvider { get; set; }
        // FK → Provider (referring clinician)

        [Required]
        public string ToProviderInfoJSON { get; set; } = string.Empty;
        // JSON: specialist name, facility, contact details

        public DateTime SentAt { get; set; } = DateTime.UtcNow;

        public DateTime? AcceptedAt { get; set; }

        public Guid? AppointmentID { get; set; }
        // FK → Appointment (resulting appointment if booked; nullable)

        [Required, MaxLength(20)]
        public string Status { get; set; } = "Sent";
        // Sent | Accepted | Scheduled | Completed | Declined

        // Navigation
        [ForeignKey(nameof(EncounterID))]
        public Encounter Encounter { get; set; } = null!;

        [ForeignKey(nameof(FromProvider))]
        public Provider FromProviderNav { get; set; } = null!;

        [ForeignKey(nameof(AppointmentID))]
        public Appointment? Appointment { get; set; }
    }
}
