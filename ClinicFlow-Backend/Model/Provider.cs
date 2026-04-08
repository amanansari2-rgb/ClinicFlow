using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClinicFlow_Backend.Model
{
    public class Provider
    {
        [Key]
        public Guid ProviderID { get; set; } = Guid.NewGuid();

        [Required]
        public Guid UserID { get; set; }
        // FK → User

        [Required, MaxLength(100)]
        public string Specialty { get; set; } = string.Empty;

        public string? ClinicIDsJSON { get; set; }
        // JSON array of assigned Clinic IDs

        public string? AvailabilityJSON { get; set; }
        // JSON: weekly availability windows

        public int MaxDailySlots { get; set; } = 20;

        [Required, MaxLength(20)]
        public string Status { get; set; } = "Active";
        // Active | OnLeave | Inactive

        // Navigation
        [ForeignKey(nameof(UserID))]
        public User User { get; set; } = null!;

        public ICollection<Appointment> Appointments { get; set; } = [];
        public ICollection<Encounter> Encounters { get; set; } = [];
        public ICollection<Waitlist> Waitlists { get; set; } = [];
        public ICollection<Referral> Referrals { get; set; } = [];
    }
}
