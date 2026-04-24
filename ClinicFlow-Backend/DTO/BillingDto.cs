namespace ClinicFlow_Backend.DTO
{
    // ─────────────────────────────────────────────────────────────
    // CHARGE DTOs
    // ─────────────────────────────────────────────────────────────

    public class ChargeDto
    {
        public Guid ChargeID { get; set; }
        public Guid EncounterID { get; set; }
        public string CPTCodesJSON { get; set; } = string.Empty;
        public string ICDCodesJSON { get; set; } = string.Empty;
        public int Units { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Amount { get; set; }
        public DateTime BilledAt { get; set; }
        public string Status { get; set; } = string.Empty;
    }

    public class CreateChargeDto
    {
        public Guid EncounterID { get; set; }
        public string CPTCodesJSON { get; set; } = string.Empty;
        public string ICDCodesJSON { get; set; } = string.Empty;
        public int Units { get; set; } = 1;
        public decimal UnitPrice { get; set; }
    }

    public class UpdateChargeDto
    {
        public string? CPTCodesJSON { get; set; }
        public string? ICDCodesJSON { get; set; }
        public int? Units { get; set; }
        public decimal? UnitPrice { get; set; }
        public string? Status { get; set; }
    }

    // ─────────────────────────────────────────────────────────────
    // INVOICE DTOs
    // ─────────────────────────────────────────────────────────────

    public class InvoiceDto
    {
        public Guid InvoiceID { get; set; }
        public Guid PatientID { get; set; }
        public string EncounterIDsJSON { get; set; } = string.Empty;
        public string ChargesJSON { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public string Currency { get; set; } = string.Empty;
        public DateTime IssuedAt { get; set; }
        public DateOnly DueDate { get; set; }
        public string Status { get; set; } = string.Empty;
        public string? InvoiceURI { get; set; }
    }

    public class CreateInvoiceDto
    {
        public Guid PatientID { get; set; }
        public string EncounterIDsJSON { get; set; } = string.Empty;
        public DateOnly DueDate { get; set; }
        public string Currency { get; set; } = "USD";
    }

    // ─────────────────────────────────────────────────────────────
    // PAYMENT DTOs
    // ─────────────────────────────────────────────────────────────

    public class PaymentDto
    {
        public Guid PaymentID { get; set; }
        public Guid InvoiceID { get; set; }
        public Guid PatientID { get; set; }
        public decimal Amount { get; set; }
        public string Method { get; set; } = string.Empty;
        public DateTime PaidAt { get; set; }
        public string Status { get; set; } = string.Empty;
    }

    public class CreatePaymentDto
    {
        public Guid InvoiceID { get; set; }
        public Guid PatientID { get; set; }
        public decimal Amount { get; set; }
        public string Method { get; set; } = string.Empty; // Cash | Card | Insurance | Online | Check
    }
}
