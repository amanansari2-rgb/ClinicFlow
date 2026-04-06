using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClinicFlow_Backend.Model
{
    public class Waitlist
    {
        [Key]
        public Guid WaitID { get; set; } = Guid.NewGuid();

        [Required]
        public Guid PatientID { get; set; }
        // FK → Patient

        [Required]
        public Guid ProviderID { get; set; }
        // FK → Provider

        [Required]
        public DateTime RequestedWindowStart { get; set; }

        [Required]
        public DateTime RequestedWindowEnd { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Required, MaxLength(20)]
        public string Status { get; set; } = "Active";
        // Active | Offered | Booked | Expired | Cancelled

        // Navigation
        [ForeignKey(nameof(PatientID))]
        public Patient Patient { get; set; } = null!;

        [ForeignKey(nameof(ProviderID))]
        public Provider Provider { get; set; } = null!;
    }
}
