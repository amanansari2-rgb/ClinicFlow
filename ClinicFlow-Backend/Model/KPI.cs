using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClinicFlow_Backend.Model
{
    public class KPI
    {
        [Key]
        public Guid KPIID { get; set; } = Guid.NewGuid();

        [Required, MaxLength(150)]
        public string Name { get; set; } = string.Empty;
        // e.g. Average Wait Time, No-Show Rate

        [Required]
        public string Definition { get; set; } = string.Empty;
        // Formula or computation rule

        [Column(TypeName = "decimal(10,2)")]
        public decimal? Target { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal? CurrentValue { get; set; }

        [Required, MaxLength(30)]
        public string ReportingPeriod { get; set; } = string.Empty;
        // Daily | Weekly | Monthly
    }
}
