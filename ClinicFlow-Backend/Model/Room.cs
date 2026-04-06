using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClinicFlow_Backend.Model
{
    public class Room
    {
        [Key]
        public Guid RoomID { get; set; } = Guid.NewGuid();

        [Required]
        public Guid ClinicID { get; set; }
        // FK → Clinic

        [Required, MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        public int Capacity { get; set; } = 1;

        public string? ResourcesJSON { get; set; }
        // JSON: equipment list available in the room

        [Required, MaxLength(20)]
        public string Status { get; set; } = "Available";
        // Available | Occupied | Maintenance

        // Navigation
        [ForeignKey(nameof(ClinicID))]
        public Clinic Clinic { get; set; } = null!;
    }
}
