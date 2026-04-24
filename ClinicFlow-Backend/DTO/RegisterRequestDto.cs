namespace ClinicFlow_Backend.DTO
{
    public class RegisterRequestDto
    {
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string? Phone { get; set; }

        // Required clinical fields for Patient record
        public DateOnly DOB { get; set; }
        public string Gender { get; set; } = string.Empty;
        // Male | Female | Other | Prefer Not To Say

        // Optional — can be filled in later via PUT /patients/{id}
        public string? ContactInfoJSON { get; set; }
        public string? AddressJSON { get; set; }
        public string? InsuranceInfoJSON { get; set; }
    }
}
