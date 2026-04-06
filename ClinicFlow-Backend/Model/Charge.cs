using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClinicFlow_Backend.Model
{
    public class Charge
    {
        [Key]
        public Guid ChargeID { get; set; } = Guid.NewGuid();

        [Required]
        public Guid EncounterID { get; set; }
        // FK → Encounter

        [Required]
        public string CPTCodesJSON { get; set; } = string.Empty;
        // JSON array of CPT procedure codes

        [Required]
        public string ICDCodesJSON { get; set; } = string.Empty;
        // JSON array of ICD-10 diagnosis codes

        public int Units { get; set; } = 1;

        [Column(TypeName = "decimal(10,2)")]
        public decimal UnitPrice { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal Amount { get; set; }
        // Computed: Units × UnitPrice

        public DateTime BilledAt { get; set; } = DateTime.UtcNow;

        [Required, MaxLength(20)]
        public string Status { get; set; } = "Draft";
        // Draft | Submitted | Approved | Voided

        // Navigation
        [ForeignKey(nameof(EncounterID))]
        public Encounter Encounter { get; set; } = null!;
    }
}
