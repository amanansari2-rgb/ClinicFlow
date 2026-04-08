using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClinicFlow_Backend.Model
{
    public class Order
    {
        [Key]
        public Guid OrderID { get; set; } = Guid.NewGuid();

        [Required]
        public Guid EncounterID { get; set; }
        // FK → Encounter

        [Required]
        public Guid OrderedBy { get; set; }
        // FK → Identity (ordering clinician)

        [Required, MaxLength(30)]
        public string OrderType { get; set; } = string.Empty;
        // Lab | Imaging | Consult | Procedure

        [Required]
        public string OrderDetailsJSON { get; set; } = string.Empty;
        // JSON: test name, instructions, priority

        public DateTime OrderedAt { get; set; } = DateTime.UtcNow;

        public DateTime? ExpectedAt { get; set; }

        [Required, MaxLength(20)]
        public string Status { get; set; } = "Ordered";
        // Ordered | InProgress | Completed | Cancelled

        // Navigation
        [ForeignKey(nameof(EncounterID))]
        public Encounter Encounter { get; set; } = null!;

        [ForeignKey(nameof(OrderedBy))]
        public User OrderedByUser { get; set; } = null!;
    }
}
