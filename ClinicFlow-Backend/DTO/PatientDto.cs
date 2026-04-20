namespace ClinicFlow_Backend.DTO
{
    // Returned by GET /patients and GET /patients/{id}
    public class PatientDto
    {
        public Guid PatientID { get; set; }
        public Guid UserID { get; set; }
        public string MRN { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public DateOnly DOB { get; set; }
        public string Gender { get; set; } = string.Empty;
        public string? ContactInfoJSON { get; set; }
        public string? AddressJSON { get; set; }
        public string? InsuranceInfoJSON { get; set; }
        public string ConsentStatus { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }

    // Used by POST /patients
    public class CreatePatientDto
    {
        public Guid UserID { get; set; }
        public string MRN { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public DateOnly DOB { get; set; }
        public string Gender { get; set; } = string.Empty;
        public string? ContactInfoJSON { get; set; }
        public string? AddressJSON { get; set; }
        public string? InsuranceInfoJSON { get; set; }
    }

    // Used by PUT /patients/{id}
    // MRN and UserID are not updatable after registration
    public class UpdatePatientDto
    {
        public string Name { get; set; } = string.Empty;
        public DateOnly DOB { get; set; }
        public string Gender { get; set; } = string.Empty;
        public string? ContactInfoJSON { get; set; }
        public string? AddressJSON { get; set; }
        public string? InsuranceInfoJSON { get; set; }
        public string ConsentStatus { get; set; } = string.Empty;
    }
}