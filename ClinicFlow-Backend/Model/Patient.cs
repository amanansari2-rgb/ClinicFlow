using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClinicFlow_Backend.Model
{
    public class Patient
    {
        [Key]
        public Guid PatientID { get; set; } = Guid.NewGuid();

        [Required]
        public Guid UserID { get; set; }
        // FK → User

        [Required, MaxLength(30)]
        public string MRN { get; set; } = string.Empty;
        // Medical Record Number - unique per patient

        [Required, MaxLength(150)]
        public string Name { get; set; } = string.Empty;

        [Required]
        public DateOnly DOB { get; set; }

        [Required, MaxLength(20)]
        public string Gender { get; set; } = string.Empty;
        // Male | Female | Other | Prefer Not To Say

        public string? ContactInfoJSON { get; set; }
        // JSON: phone, email, emergency contact
       
        public string? AddressJSON { get; set; }
        // JSON: street, city, state, postal code

        public string? InsuranceInfoJSON { get; set; }
        // JSON: payer name, policy number, group ID

        [Required, MaxLength(20)]
        public string ConsentStatus { get; set; } = "Pending";
        // Pending | Signed | Revoked

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation
        [ForeignKey(nameof(UserID))]
        public User User { get; set; } = null!;

        public ICollection<IntakeForm> IntakeForms { get; set; } = [];
        public ICollection<Appointment> Appointments { get; set; } = [];
        public ICollection<Encounter> Encounters { get; set; } = [];
        public ICollection<Invoice> Invoices { get; set; } = [];
        public ICollection<Payment> Payments { get; set; } = [];
        public ICollection<Waitlist> Waitlists { get; set; } = [];
    }
}
