using ClinicFlow_Backend.Model;

namespace ClinicFlow_Backend.Repositories.Interface
{
    public interface IBillingRepository
    {
        // ── Charges ──────────────────────────────────────────────
        Task<IEnumerable<Charge>> GetChargesAsync(int page, int pageSize);
        Task<Charge?> GetChargeByIdAsync(Guid id);
        Task<Charge> CreateChargeAsync(Charge charge);
        Task<Charge?> UpdateChargeAsync(Guid id, Charge updated);

        // ── Invoices ─────────────────────────────────────────────
        Task<IEnumerable<Invoice>> GetInvoicesAsync(int page, int pageSize);
        Task<Invoice?> GetInvoiceByIdAsync(Guid id);
        Task<Invoice> CreateInvoiceAsync(Invoice invoice);

        // ── Payments ─────────────────────────────────────────────
        Task<IEnumerable<Payment>> GetPaymentsAsync(int page, int pageSize);
        Task<Payment> CreatePaymentAsync(Payment payment);
    }
}
