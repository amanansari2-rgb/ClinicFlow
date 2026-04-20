using ClinicFlow_Backend.Data;
using ClinicFlow_Backend.Model;
using ClinicFlow_Backend.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace ClinicFlow_Backend.Repositories.Implementation
{
    public class PatientRepository:IPatientRepository
    {
        private readonly AppDbContext _context;

        public PatientRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Patient>> GetPatientsAsync()
        {
            return await _context.Patients.ToListAsync();
        }
        public async Task<Patient?> GetPatientAsync(Guid id)
        {
            return await _context.Patients.FindAsync(id);
        }
        public async Task<bool> PutPatientAsync(Guid id, Patient patient)
        {
            if (id != patient.PatientID)
                return false;

            _context.Entry(patient).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await PatientExistsAsync(id))
                    return false;

                throw;
            }
        }
        public async Task<Patient> PostPatientAsync(Patient patient)
        {
            _context.AddAsync(patient);

            await _context.SaveChangesAsync();

            return patient;
        }
        public async Task<bool> DeletePatientAsync(Guid id)
        {
            var patient = await _context.Patients.FindAsync(id);
            if(patient == null)
            {
                return false;
            }

            _context.Patients.Remove(patient);
            _context.SaveChanges();
            return true;
        }

        private async Task<bool> PatientExistsAsync(Guid id)
        {
            return await _context.Patients.AnyAsync(e => e.PatientID == id);
        }


        //IntakeForm Methods
        public async Task<IEnumerable<IntakeForm>> GetIntakeFormsByPatientAsync(Guid patientId)
            => await _context.IntakeForms.Where(f => f.PatientID == patientId).ToListAsync();

        public async Task<IntakeForm?> GetIntakeFormAsync(Guid id)
            => await _context.IntakeForms.FindAsync(id);

        public async Task<IntakeForm> AddIntakeFormAsync(IntakeForm form)
        {
            _context.IntakeForms.Add(form);
            await _context.SaveChangesAsync();
            return form;
        }

        public async Task<bool> UpdateIntakeFormAsync(Guid id, IntakeForm form)
        {
            if (id != form.FormID) return false;
            _context.Entry(form).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _context.IntakeForms.AnyAsync(e => e.FormID == id)) return false;
                throw;
            }
        }

        public async Task<bool> DeleteIntakeFormAsync(Guid id)
        {
            var form = await _context.IntakeForms.FindAsync(id);
            if (form == null) return false;
            _context.IntakeForms.Remove(form);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
