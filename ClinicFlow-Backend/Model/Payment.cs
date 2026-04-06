using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClinicFlow_Backend.Model
{
    public class Payment
    {
        [Key]
        public Guid PaymentID { get; set; } = Guid.NewGuid();

        [Required]
        public Guid InvoiceID { get; set; }
        // FK → Invoice

        [Required]
        public Guid PatientID { get; set; }
        // FK → Patient

        [Column(TypeName = "decimal(10,2)")]
        public decimal Amount { get; set; }

        [Required, MaxLength(30)]
        public string Method { get; set; } = string.Empty;
        // Cash | Card | Insurance | Online | Check

        public DateTime PaidAt { get; set; } = DateTime.UtcNow;

        [Required, MaxLength(20)]
        public string Status { get; set; } = "Pending";
        // Pending | Completed | Refunded | Failed

        // Navigation
        [ForeignKey(nameof(InvoiceID))]
        public Invoice Invoice { get; set; } = null!;

        [ForeignKey(nameof(PatientID))]
        public Patient Patient { get; set; } = null!;
    }
}
