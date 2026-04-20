namespace ClinicFlow_Backend.DTO
{
    // Returned by GET /patients/{id}/intake-forms and GET /intake-forms/{id}
    public class IntakeFormDto
    {
        public Guid FormID { get; set; }
        public Guid PatientID { get; set; }
        public string VisitReason { get; set; } = string.Empty;
        public string? SymptomsJSON { get; set; }
        public string? AllergiesJSON { get; set; }
        public string? MedicationsJSON { get; set; }
        public DateTime? CompletedAt { get; set; }
        public string Source { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
    }

    // Used by POST /patients/{id}/intake-forms
    public class CreateIntakeFormDto
    {
        public string VisitReason { get; set; } = string.Empty;
        public string? SymptomsJSON { get; set; }
        public string? AllergiesJSON { get; set; }
        public string? MedicationsJSON { get; set; }
        public string Source { get; set; } = "Portal";  // Portal | Kiosk | Staff
    }

    // Used by PUT /intake-forms/{id}
    public class UpdateIntakeFormDto
    {
        public string VisitReason { get; set; } = string.Empty;
        public string? SymptomsJSON { get; set; }
        public string? AllergiesJSON { get; set; }
        public string? MedicationsJSON { get; set; }
        public string Source { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;  // Draft | Submitted | Reviewed
    }
}
