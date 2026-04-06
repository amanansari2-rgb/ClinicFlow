using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClinicFlow_Backend.Model
{
    public class IntakeForm
    {
        [Key]
        public Guid FormID { get; set; } = Guid.NewGuid();

        [Required]
        public Guid PatientID { get; set; }
        // FK → Patient

        [Required, MaxLength(300)]
        public string VisitReason { get; set; } = string.Empty;

        public string? SymptomsJSON { get; set; }
        // JSON array of reported symptoms

        public string? AllergiesJSON { get; set; }
        // JSON array of allergy records

        public string? MedicationsJSON { get; set; }
        // JSON array of current medications

        public DateTime? CompletedAt { get; set; }

        [Required, MaxLength(20)]
        public string Source { get; set; } = "Portal";
        // Portal | Kiosk | Staff

        [Required, MaxLength(20)]
        public string Status { get; set; } = "Draft";
        // Draft | Submitted | Reviewed

        // Navigation
        [ForeignKey(nameof(PatientID))]
        public Patient Patient { get; set; } = null!;
    }
}
