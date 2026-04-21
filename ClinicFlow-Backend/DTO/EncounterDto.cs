using System.ComponentModel.DataAnnotations;

namespace ClinicFlow_Backend.DTO
{
    // ─── ENCOUNTER RESPONSE DTO ──────────────────────────────────────────────
    public class EncounterDto
    {
        public Guid EncounterID { get; set; }
        public Guid AppID { get; set; }
        public Guid PatientID { get; set; }
        public Guid ProviderID { get; set; }
        public DateTime StartAt { get; set; }
        public DateTime? EndAt { get; set; }
        public string VisitType { get; set; } = string.Empty;
        public string ChiefComplaint { get; set; } = string.Empty;
        public string? EncounterNoteURI { get; set; }
        public string Status { get; set; } = string.Empty;
    }

    // ─── ENCOUNTER CREATE REQUEST DTO ────────────────────────────────────────
    public class CreateEncounterDto
    {
        [Required]
        public Guid AppID { get; set; }

        [Required]
        public Guid PatientID { get; set; }

        [Required]
        public Guid ProviderID { get; set; }

        [Required]
        public DateTime StartAt { get; set; }

        [Required, MaxLength(30)]
        public string VisitType { get; set; } = string.Empty;

        [Required, MaxLength(500)]
        public string ChiefComplaint { get; set; } = string.Empty;
    }

    // ─── ENCOUNTER UPDATE REQUEST DTO ────────────────────────────────────────
    public class UpdateEncounterDto
    {
        public DateTime? EndAt { get; set; }

        [Required, MaxLength(30)]
        public string VisitType { get; set; } = string.Empty;

        [Required, MaxLength(500)]
        public string ChiefComplaint { get; set; } = string.Empty;

        [MaxLength(500)]
        public string? EncounterNoteURI { get; set; }

        [Required, MaxLength(20)]
        public string Status { get; set; } = string.Empty;
    }

    // ─── ORDER RESPONSE DTO ──────────────────────────────────────────────────
    public class OrderDto
    {
        public Guid OrderID { get; set; }
        public Guid EncounterID { get; set; }
        public Guid OrderedBy { get; set; }
        public string OrderType { get; set; } = string.Empty;
        public string OrderDetailsJSON { get; set; } = string.Empty;
        public DateTime OrderedAt { get; set; }
        public DateTime? ExpectedAt { get; set; }
        public string Status { get; set; } = string.Empty;
    }

    // ─── ORDER CREATE REQUEST DTO ────────────────────────────────────────────
    public class CreateOrderDto
    {
        [Required]
        public Guid OrderedBy { get; set; }

        [Required, MaxLength(30)]
        public string OrderType { get; set; } = string.Empty;

        [Required]
        public string OrderDetailsJSON { get; set; } = string.Empty;

        public DateTime? ExpectedAt { get; set; }
    }

    // ─── REFERRAL RESPONSE DTO ───────────────────────────────────────────────
    public class ReferralDto
    {
        public Guid ReferralID { get; set; }
        public Guid EncounterID { get; set; }
        public Guid FromProvider { get; set; }
        public string ToProviderInfoJSON { get; set; } = string.Empty;
        public DateTime SentAt { get; set; }
        public DateTime? AcceptedAt { get; set; }
        public Guid? AppointmentID { get; set; }
        public string Status { get; set; } = string.Empty;
    }

    // ─── REFERRAL CREATE REQUEST DTO ─────────────────────────────────────────
    public class CreateReferralDto
    {
        [Required]
        public Guid FromProvider { get; set; }

        [Required]
        public string ToProviderInfoJSON { get; set; } = string.Empty;

        public Guid? AppointmentID { get; set; }
    }
}
