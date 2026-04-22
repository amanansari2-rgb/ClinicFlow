using System.ComponentModel.DataAnnotations;

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
        [Required]
        public Guid PatientID { get; set; }

        [Required]
        public Guid ProviderID { get; set; }

        [Required]
        public Guid ClinicID { get; set; }

        [Required]
        public DateTime ScheduledAt { get; set; }

        public int DurationMinutes { get; set; } = 30;

        [Required]
        public string Mode { get; set; } = string.Empty;    // InPerson | Telehealth

        [Required]
        public string Reason { get; set; } = string.Empty;

        [Required]
        public Guid CreatedBy { get; set; }
    }

    // Used by PUT /appointments/{id}
    public class UpdateAppointmentDto
    {
        [Required]
        public DateTime ScheduledAt { get; set; }

        public int DurationMinutes { get; set; }

        [Required]
        public string Mode { get; set; } = string.Empty;

        [Required]
        public string Reason { get; set; } = string.Empty;

        [Required]
        public string Status { get; set; } = string.Empty;  // Scheduled | Confirmed | CheckedIn | Completed | Cancelled | NoShow
    }
}
