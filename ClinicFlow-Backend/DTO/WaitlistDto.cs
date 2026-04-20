namespace ClinicFlow_Backend.DTO
{
    // Returned by GET /appointments/waitlist and GET /appointments/waitlist/{id}
    public class WaitlistDto
    {
        public Guid WaitID { get; set; }
        public Guid PatientID { get; set; }
        public Guid ProviderID { get; set; }
        public DateTime RequestedWindowStart { get; set; }
        public DateTime RequestedWindowEnd { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Status { get; set; } = string.Empty;
    }

    // Used by POST /appointments/waitlist
    public class CreateWaitlistDto
    {
        public Guid PatientID { get; set; }
        public Guid ProviderID { get; set; }
        public DateTime RequestedWindowStart { get; set; }
        public DateTime RequestedWindowEnd { get; set; }
    }

    // Used by PUT /appointments/waitlist/{id}
    public class UpdateWaitlistDto
    {
        public DateTime RequestedWindowStart { get; set; }
        public DateTime RequestedWindowEnd { get; set; }
        public string Status { get; set; } = string.Empty;  // Active | Offered | Booked | Expired | Cancelled
    }
}