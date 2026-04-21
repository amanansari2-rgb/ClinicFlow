using System.ComponentModel.DataAnnotations;

namespace ClinicFlow_Backend.Model
{
    public class Report
    {
        [Key]
        public Guid ReportID { get; set; } = Guid.NewGuid();

        [Required, MaxLength(30)]
        public string Scope { get; set; } = string.Empty;
        // Operational | Financial | Clinical

        [Required]
        public string ParametersJSON { get; set; } = string.Empty;
        // JSON: date range, clinic ID, filters

        public string? MetricsJSON { get; set; }
        // JSON: computed metric values

        [Required]
        public Guid GeneratedBy { get; set; }
        // FK → User

        public DateTime GeneratedAt { get; set; } = DateTime.UtcNow;

        [MaxLength(500)]
        public string? ReportURI { get; set; }
        // Object storage URI for the exported file

    }
}
