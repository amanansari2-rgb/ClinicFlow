using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClinicFlow_Backend.Model
{
    public class Invoice
    {
        [Key]
        public Guid InvoiceID { get; set; } = Guid.NewGuid();

        [Required]
        public Guid PatientID { get; set; }
        // FK → Patient

        [Required]
        public string EncounterIDsJSON { get; set; } = string.Empty;
        // JSON array of included Encounter IDs

        [Required]
        public string ChargesJSON { get; set; } = string.Empty;
        // JSON snapshot of charge lines at time of issuance

        [Column(TypeName = "decimal(10,2)")]
        public decimal Amount { get; set; }

        [Required, MaxLength(5)]
        public string Currency { get; set; } = "USD";
        // ISO 4217 currency code

        public DateTime IssuedAt { get; set; } = DateTime.UtcNow;

        [Required]
        public DateOnly DueDate { get; set; }

        [Required, MaxLength(20)]
        public string Status { get; set; } = "Draft";
        // Draft | Issued | PartiallyPaid | Paid | Overdue | Voided

        [MaxLength(500)]
        public string? InvoiceURI { get; set; }
        // Object storage URI for the PDF invoice

        // Navigation
        [ForeignKey(nameof(PatientID))]
        public Patient Patient { get; set; } = null!;

        public ICollection<Payment> Payments { get; set; } = [];
    }
}
