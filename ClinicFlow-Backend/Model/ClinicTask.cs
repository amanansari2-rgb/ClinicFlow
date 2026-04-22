using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace ClinicFlow_Backend.Model
{
    // Named ClinicTask to avoid conflict with System.Threading.Tasks.Task
    public class ClinicTask    {
        [Key]
        public Guid TaskID { get; set; } = Guid.NewGuid();

        [Required]
        public Guid AssignedTo { get; set; }
        // FK → User (responsible user)

        [MaxLength(50)]
        public string? RelatedEntityID { get; set; }
        // ID of related entity e.g. EncounterID, InvoiceID

        [Required, MaxLength(500)]
        public string Description { get; set; } = string.Empty;

        [Required]
        public DateTime DueDate { get; set; }

        [Required, MaxLength(20)]
        public string Priority { get; set; } = "Medium";
        // Low | Medium | High | Critical
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? CompletedAt { get; set; }

        [Required, MaxLength(20)]
        public string Status { get; set; } = "Open";
        // Open | InProgress | Completed | Overdue
        
        // Navigation
        [ForeignKey(nameof(AssignedTo))]
        public User AssignedToUser { get; set; } = null!;
    }
}