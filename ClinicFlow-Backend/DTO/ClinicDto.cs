using System.ComponentModel.DataAnnotations;

namespace ClinicFlow_Backend.DTO
{
    // ─── RESPONSE DTO ─────────────────────────────────────────────────────────
    // Returned by GET /clinics and GET /clinics/{id}
    public class ClinicDto
    {
        public Guid ClinicID { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? AddressJSON { get; set; }
        // JSON: street, city, state, postal code
        public string Status { get; set; } = string.Empty;
    }

    // ─── CREATE REQUEST DTO ───────────────────────────────────────────────────
    // Used by POST /clinics
    public class CreateClinicDto
    {
        [Required, MaxLength(150)]
        public string Name { get; set; } = string.Empty;

        public string? AddressJSON { get; set; }
        // JSON: street, city, state, postal code
    }

    // ─── UPDATE REQUEST DTO ───────────────────────────────────────────────────
    // Used by PUT /clinics/{id}
    public class UpdateClinicDto
    {
        [Required, MaxLength(150)]
        public string Name { get; set; } = string.Empty;

        public string? AddressJSON { get; set; }
        // JSON: street, city, state, postal code

        [Required, MaxLength(20)]
        public string Status { get; set; } = string.Empty;
        // Active | Inactive
    }
}
