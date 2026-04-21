using System.ComponentModel.DataAnnotations;

namespace ClinicFlow_Backend.Model
{
    public class Notification
    {
        [Key]
        public Guid NotificationID { get; set; } = Guid.NewGuid();

        [Required]
        public Guid UserID { get; set; }
        // FK → User (recipient)

        [MaxLength(50)]
        public string? EntityID { get; set; }
        // ID of the related entity e.g. AppID, InvoiceID

        [Required, MaxLength(500)]
        public string Message { get; set; } = string.Empty;

        [Required, MaxLength(30)]
        public string Category { get; set; } = string.Empty;
        // Appointment | Billing | Task | System

        [Required, MaxLength(20)]
        public string Severity { get; set; } = "Info";
        // Info | Warning | Critical

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? ReadAt { get; set; }

        [Required, MaxLength(20)]
        public string Status { get; set; } = "Unread";
        // Unread | Read | Dismissed

    }
}
