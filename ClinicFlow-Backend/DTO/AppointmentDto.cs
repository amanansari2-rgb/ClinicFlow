namespace ClinicFlow_Backend.DTO
{
    // Returned by GET /appointments and GET /appointments/{id}
    public class AppointmentDto
    {
        public Guid AppID { get; set; }
        public Guid PatientID { get; set; }
        public Guid ProviderID { get; set; }
        public Guid ClinicID { get; set; }
        public DateTime ScheduledAt { get; set; }
        public int DurationMinutes { get; set; }
        public string Mode { get; set; } = string.Empty;
        public string Reason { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public Guid CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    // Used by POST /appointments
    public class CreateAppointmentDto
    {
        public Guid PatientID { get; set; }
        public Guid ProviderID { get; set; }
        public Guid ClinicID { get; set; }
        public DateTime ScheduledAt { get; set; }
        public int DurationMinutes { get; set; } = 30;
        public string Mode { get; set; } = string.Empty;    // InPerson | Telehealth
        public string Reason { get; set; } = string.Empty;
        public Guid CreatedBy { get; set; }
    }

    // Used by PUT /appointments/{id}
    public class UpdateAppointmentDto
    {
        public DateTime ScheduledAt { get; set; }
        public int DurationMinutes { get; set; }
        public string Mode { get; set; } = string.Empty;
        public string Reason { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;  // Scheduled | Confirmed | CheckedIn | Completed | Cancelled | NoShow
    }
}
