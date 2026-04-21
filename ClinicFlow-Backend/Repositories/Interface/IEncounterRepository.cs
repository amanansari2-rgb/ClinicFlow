using ClinicFlow_Backend.Model;

namespace ClinicFlow_Backend.Repositories.Interface
{
    public interface IEncounterRepository
    {
        // ── Encounters ───────────────────────────────────────────────────
        Task<IEnumerable<Encounter>> GetEncountersAsync();
        Task<Encounter?> GetEncounterAsync(Guid id);
        Task<Encounter> PostEncounterAsync(Encounter encounter);
        Task<bool> PutEncounterAsync(Guid id, Encounter encounter);

        // ── Orders (nested under Encounter) ──────────────────────────────
        Task<IEnumerable<Order>> GetOrdersByEncounterAsync(Guid encounterId);
        Task<Order> PostOrderAsync(Order order);

        // ── Referrals (nested under Encounter) ───────────────────────────
        Task<IEnumerable<Referral>> GetReferralsByEncounterAsync(Guid encounterId);
        Task<Referral> PostReferralAsync(Referral referral);
    }
}
