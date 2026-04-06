using System.ComponentModel.DataAnnotations;

namespace ClinicFlow_Backend.Model
{
    public class Clinic
    {
        [Key]
        public Guid ClinicID { get; set; } = Guid.NewGuid();

        [Required, MaxLength(150)]
        public string Name { get; set; } = string.Empty;

        public string? AddressJSON { get; set; }
        // JSON: street, city, state, postal code

        [Required, MaxLength(20)]
        public string Status { get; set; } = "Active";
        // Active | Inactive

        // Navigation
        public ICollection<Provider> Providers { get; set; } = [];
        public ICollection<Appointment> Appointments { get; set; } = [];
        public ICollection<Room> Rooms { get; set; } = [];
    }
}
