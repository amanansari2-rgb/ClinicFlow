using ClinicFlow_Backend.Data;
using ClinicFlow_Backend.Model;
using ClinicFlow_Backend.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace ClinicFlow_Backend.Repositories.Implementation
{
    public class BillingRepository : IBillingRepository
    {
        private readonly AppDbContext _context;

        public BillingRepository(AppDbContext context)
        {
            _context = context;
        }

        // ── Charges ──────────────────────────────────────────────

        public async Task<IEnumerable<Charge>> GetChargesAsync(int page, int pageSize)
        {  
            return await _context.Charges
                .OrderByDescending(c => c.BilledAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<Charge?> GetChargeByIdAsync(Guid id)
        {
            return await _context.Charges.FindAsync(id);
        }

        public async Task<Charge> CreateChargeAsync(Charge charge)
        {
            charge.Amount = charge.Units * charge.UnitPrice;
            _context.Charges.Add(charge);
            await _context.SaveChangesAsync();
            return charge;
        }

        public async Task<Charge?> UpdateChargeAsync(Guid id, Charge updated)
        {
            var existing = await _context.Charges.FindAsync(id);
            if (existing is null) return null;

            if (updated.CPTCodesJSON is not null) existing.CPTCodesJSON = updated.CPTCodesJSON;
            if (updated.ICDCodesJSON is not null) existing.ICDCodesJSON = updated.ICDCodesJSON;
            if (updated.Units > 0) existing.Units = updated.Units;
            if (updated.UnitPrice > 0) existing.UnitPrice = updated.UnitPrice;
            if (!string.IsNullOrWhiteSpace(updated.Status)) existing.Status = updated.Status;

            existing.Amount = existing.Units * existing.UnitPrice;

            await _context.SaveChangesAsync();
            return existing;
        }

        // ── Invoices ─────────────────────────────────────────────

        public async Task<IEnumerable<Invoice>> GetInvoicesAsync(int page, int pageSize)
        {
            return await _context.Invoices
                .Include(i => i.Patient)
                .OrderByDescending(i => i.IssuedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<Invoice?> GetInvoiceByIdAsync(Guid id)
        {
            return await _context.Invoices
                .Include(i => i.Patient)
                .Include(i => i.Payments)
                .FirstOrDefaultAsync(i => i.InvoiceID == id);
        }

        public async Task<Invoice> CreateInvoiceAsync(Invoice invoice)
        {
            _context.Invoices.Add(invoice);
            await _context.SaveChangesAsync();
            return invoice;
        }

        // ── Payments ─────────────────────────────────────────────

        public async Task<IEnumerable<Payment>> GetPaymentsAsync(int page, int pageSize)
        {
            return await _context.Payments
                .OrderByDescending(p => p.PaidAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<Payment> CreatePaymentAsync(Payment payment)
        {
            payment.Status = "Completed";
            _context.Payments.Add(payment);

            // Auto-update invoice status
            var invoice = await _context.Invoices
                .Include(i => i.Payments)
                .FirstOrDefaultAsync(i => i.InvoiceID == payment.InvoiceID);

            if (invoice is not null)
            {
                var totalPaid = invoice.Payments.Sum(p => p.Amount) + payment.Amount;
                invoice.Status = totalPaid >= invoice.Amount ? "Paid" : "PartiallyPaid";
            }

            await _context.SaveChangesAsync();
            return payment;
        }
    }
}
