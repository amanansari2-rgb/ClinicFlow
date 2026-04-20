using ClinicFlow_Backend.Data;
using ClinicFlow_Backend.Model;
using ClinicFlow_Backend.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace ClinicFlow_Backend.Repositories.Implementation
{
    public class ClinicRepository : IClinicRepository
    {
        private readonly AppDbContext _context;

        public ClinicRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Clinic>> GetClinicsAsync()
        {
            return await _context.Clinics.ToListAsync();
        }

        public async Task<Clinic?> GetClinicAsync(Guid id)
        {
            return await _context.Clinics.FindAsync(id);
        }

        public async Task<Clinic> PostClinicAsync(Clinic clinic)
        {
            _context.Clinics.Add(clinic);
            await _context.SaveChangesAsync();
            return clinic;
        }

        public async Task<bool> PutClinicAsync(Guid id, Clinic clinic)
        {
            if (id != clinic.ClinicID)
                return false;

            _context.Entry(clinic).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await ClinicExistsAsync(id))
                    return false;

                throw;
            }
        }

        public async Task<bool> DeleteClinicAsync(Guid id)
        {
            var clinic = await _context.Clinics.FindAsync(id);
            if (clinic == null)
                return false;

            _context.Clinics.Remove(clinic);
            await _context.SaveChangesAsync();
            return true;
        }

        private async Task<bool> ClinicExistsAsync(Guid id)
        {
            return await _context.Clinics.AnyAsync(e => e.ClinicID == id);
        }
    }
}
