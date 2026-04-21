namespace ClinicFlow_Backend.DTO
{
    // Returned by GET /providers and GET /providers/{id}
    public class ProviderDto
    {
        public Guid ProviderID { get; set; }
        public Guid UserID { get; set; }
        public string Specialty { get; set; } = string.Empty;
        public string? ClinicIDsJSON { get; set; }
        public string? AvailabilityJSON { get; set; }
        public int MaxDailySlots { get; set; }
        public string Status { get; set; } = string.Empty;
    }

    // Used by POST /providers
    public class CreateProviderDto
    {
        public Guid UserID { get; set; }
        public string Specialty { get; set; } = string.Empty;
        public string? ClinicIDsJSON { get; set; }
        public string? AvailabilityJSON { get; set; }
        public int MaxDailySlots { get; set; } = 20;
    }

    // Used by PUT /providers/{id}
    public class UpdateProviderDto
    {
        public string Specialty { get; set; } = string.Empty;
        public string? ClinicIDsJSON { get; set; }
        public string? AvailabilityJSON { get; set; }
        public int MaxDailySlots { get; set; }
        public string Status { get; set; } = string.Empty;  // Active | OnLeave | Inactive
    }
}
