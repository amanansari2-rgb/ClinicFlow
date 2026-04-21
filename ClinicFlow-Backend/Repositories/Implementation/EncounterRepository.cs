using ClinicFlow_Backend.Data;
using ClinicFlow_Backend.Model;
using ClinicFlow_Backend.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace ClinicFlow_Backend.Repositories.Implementation
{
    public class EncounterRepository : IEncounterRepository
    {
        private readonly AppDbContext _context;

        public EncounterRepository(AppDbContext context)
        {
            _context = context;
        }

        // ── Encounters ───────────────────────────────────────────────────

        public async Task<IEnumerable<Encounter>> GetEncountersAsync()
        {
            return await _context.Encounters.ToListAsync();
        }

        public async Task<Encounter?> GetEncounterAsync(Guid id)
        {
            return await _context.Encounters.FindAsync(id);
        }

        public async Task<Encounter> PostEncounterAsync(Encounter encounter)
        {
            _context.Encounters.Add(encounter);
            await _context.SaveChangesAsync();
            return encounter;
        }

        public async Task<bool> PutEncounterAsync(Guid id, Encounter encounter)
        {
            if (id != encounter.EncounterID)
                return false;

            _context.Entry(encounter).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await EncounterExistsAsync(id))
                    return false;

                throw;
            }
        }

        // ── Orders ───────────────────────────────────────────────────────

        public async Task<IEnumerable<Order>> GetOrdersByEncounterAsync(Guid encounterId)
        {
            return await _context.Orders
                .Where(o => o.EncounterID == encounterId)
                .ToListAsync();
        }

        public async Task<Order> PostOrderAsync(Order order)
        {
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
            return order;
        }

        // ── Referrals ────────────────────────────────────────────────────

        public async Task<IEnumerable<Referral>> GetReferralsByEncounterAsync(Guid encounterId)
        {
            return await _context.Referrals
                .Where(r => r.EncounterID == encounterId)
                .ToListAsync();
        }

        public async Task<Referral> PostReferralAsync(Referral referral)
        {
            _context.Referrals.Add(referral);
            await _context.SaveChangesAsync();
            return referral;
        }

        // ── Helpers ──────────────────────────────────────────────────────

        private async Task<bool> EncounterExistsAsync(Guid id)
        {
            return await _context.Encounters.AnyAsync(e => e.EncounterID == id);
        }
    }
}
