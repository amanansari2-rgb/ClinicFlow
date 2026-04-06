using System.ComponentModel.DataAnnotations;

namespace ClinicFlow_Backend.Model
{
    public class AuditPackage
    {
        [Key]
        public Guid PackageID { get; set; } = Guid.NewGuid();

        [Required]
        public DateOnly PeriodStart { get; set; }

        [Required]
        public DateOnly PeriodEnd { get; set; }

        [Required]
        public string ContentsJSON { get; set; } = string.Empty;
        // JSON: included log IDs and sections

        public DateTime GeneratedAt { get; set; } = DateTime.UtcNow;

        [MaxLength(500)]
        public string? PackageURI { get; set; }
        // Object storage URI for the exported archive
    }
}
